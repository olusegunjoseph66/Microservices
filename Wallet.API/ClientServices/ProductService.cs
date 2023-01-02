//using Shared.Utilities.Constants;
using Shared.Utilities.DTO.Pagination;
using System;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Product.Application.DTOs.APIDataFormatters;
using Product.Application.ViewModels.Responses;
using Shared.Data.Repository;
using Product.Application.ViewModels.Requests;
using Shared.Data.Repository.Interfaces;
using Product.Application.CodeFactories;
using System.Threading;
using Product.Application.Constants;
using Product.Application.ViewModels.QueryFilters;
using Product.Application.Enums;
using Shared.ExternalServices.APIServices;
using Shared.Data.Models;
using Shared.Utilities.Helpers;
using Microsoft.Extensions.Configuration;
using Shared.ExternalServices.Configurations;
using System.Linq;

public class ProductService : IProductService
{
	private readonly IProductRepository _productRepository;
    private readonly IAzureBlobStorageUpload _azureBlobStorage;
    private readonly IProductImagesRepository _productImagesRepository;
    private readonly IProductStatusRepository _productStatusRepository;
    public readonly IDmsAzureMessageBus _messageBus;
    private readonly IConfiguration _configuration;
    private readonly string productupdatedMessageTopic;
    public readonly string productImageuploadedMessageTopic;
    private readonly string productDeletedMessageTopic;    
    public ProductService(IProductRepository productRepository, IConfiguration configuration, IDmsAzureMessageBus messageBus, IProductImagesRepository productImagesRepository, IProductStatusRepository productStatusRepository , IAzureBlobStorageUpload azureBlobStorage
        )
	{
        _configuration = configuration;
        _messageBus = messageBus;
        _productRepository = productRepository;
        _productImagesRepository = productImagesRepository;
        _productStatusRepository = productStatusRepository;
        _azureBlobStorage = azureBlobStorage;


        productupdatedMessageTopic = _configuration.GetValue<string>("ProductUpdatedMessageTopic");
        productImageuploadedMessageTopic = _configuration.GetValue<string>("ProductImageuploadedMessageTopic");
        productDeletedMessageTopic = _configuration.GetValue<string>("ProductDeletedMessageTopic");


    }



    public async Task<ApiResponse<ProductDetailsRespnse>> ViewProductById(int productId, CancellationToken cancellationToken)
    {

            var productdetail = await _productRepository.GetByIdAsync((short)productId, cancellationToken);

            ProductDetailsRespnse productdetailsoutput = new();


            if (productdetail != null)
            {
                    var ProductDetailsImages =  _productImagesRepository.Table
                        .Where(pir=>pir.ProductId == productId).ToList();

                    List<ProductDetailsImagesRespnse> productImgs = new();

                    foreach (var item in ProductDetailsImages)
                    {
                        ProductDetailsImagesRespnse imgOutput = new()
                        {
                            publicUrl = item.PublicUrl,
                            isPrimaryImage = item.IsPrimaryImage
                        };
                        productImgs.Add(imgOutput);
                    }
                        productdetailsoutput.productId = productdetail.Id;
                        productdetailsoutput.name = productdetail.Name;
                        productdetailsoutput.description = productdetail.Description;
                        productdetailsoutput.productType = productdetail.ProductType;
                        productdetailsoutput.unitOfMeasure = productdetail.UnitOfMeasure;
                        productdetailsoutput.productStatus = _productStatusRepository.Table.FirstOrDefault(ps=>ps.Id == productdetail.ProductStatusId).Name;
                        productdetailsoutput.dateModified = productdetail?.DateModified;
                        productdetailsoutput.ProductImags = productImgs;
                  
            }

        return ResponseHandler<ProductDetailsRespnse>.SuccessResponse(SuccessCodes.DefaultSuccessCode, SuccessMessages.Successfull, productdetailsoutput);
    }

    public async Task<ApiResponse<string>> ActivateDeactivateProducts(ActivateProductRequest activatedeactivateProduct, CancellationToken cancellationToken)
    {
        if (activatedeactivateProduct.productId > 0 )
        {
            var productdetail = await _productRepository.GetByIdAsync((short)activatedeactivateProduct.productId, cancellationToken);

            if(productdetail != null)
            {

                if(activatedeactivateProduct.activate == true)
                {
                    productdetail.ProductStatusId = (int)ProductStatusEnum.Active;
                }
                else
                {
                    productdetail.ProductStatusId = (int)ProductStatusEnum.InActive;
                }

                productdetail.DateModified = DateTime.UtcNow;
                //No way to get userid now hence 1
                productdetail.ModifiedByUserId = 1;

                _productRepository.Update(productdetail);

                //Azure ServiceBus

                ProductUpdatedMessage updatedProductMessage = new()
                {
                    productId = productdetail.Id, sapId = productdetail.SapId, name = productdetail.Name,
                    dateCreated = DateTime.UtcNow, description = productdetail.Description,
                    unitofmeasure = productdetail.UnitOfMeasure, productStatusId = productdetail.ProductStatusId
                };

                await _messageBus.PublishMessage(updatedProductMessage, productupdatedMessageTopic);

                return ResponseHandler<string>.SuccessResponse(SuccessMessages.Successfull, SuccessCodes.DefaultSuccessCode, "");
            }
            else
            {
                 ResponseHandler.FailureResponse(ErrorCodes.Invalidorincorrectvaluesprovided, ErrorMessages.Invalidorincorrectvaluesprovided);
            }
        }
        else
        {
             ResponseHandler.FailureResponse(ErrorCodes.Invalidorincorrectvaluesprovided, ErrorMessages.Invalidorincorrectvaluesprovided);
        }

        return ResponseHandler<string>.SuccessResponse(SuccessMessages.Successfull, SuccessCodes.DefaultSuccessCode, "");

    }


    public async Task<ApiResponse<string>> AddProductImags(UploadProductImageRequest uploadProductImg, CancellationToken cancellationToken)
    {
        if (uploadProductImg.productId > 0)
        {
            var productdetail = await _productRepository.GetByIdAsync((short)uploadProductImg.productId, cancellationToken);

            if (productdetail != null)
            {
                //var guid = New Guid();
                var guid = RandomValueGenerator.GenerateRandomDigits(8);

                var filename = $"{uploadProductImg.productId}{guid}";

                //Getting blobStorage from Configuratn microservice
                var uploadPath = SuccessCodes.uploadPaths;

                var upload = _azureBlobStorage.BlobUpload(uploadProductImg.image, uploadPath, filename);

                if (uploadProductImg.isLeadImage == true)
                {
                    var productimgs = _productImagesRepository.Table.Where(p => p.ProductId == uploadProductImg.productId
                                                               && p.IsPrimaryImage == true);
                    foreach (var item in productimgs)
                    {
                        item.IsPrimaryImage = false;
                        _productImagesRepository.Update(item);

                    }                    
                   
                }


                ProductImage newProductImg = new()
                {
                    PublicUrl = upload.publicurl,  CloudPath = upload.cloudurl, DateCreated = DateTime.UtcNow,
                    IsPrimaryImage = uploadProductImg.isLeadImage, CreatedByUserId = 1,
                    ProductId = (short)uploadProductImg.productId
                };

               await _productImagesRepository.AddAsync(newProductImg,cancellationToken);


                //Azure ServiceBus
                ProductImageUploadedMessage uploadedMessage = new()
                {
                    productId = newProductImg.ProductId,
                    createdByUserId = 1,
                    dateCreated = DateTime.UtcNow,
                    cloudPath = upload.cloudurl,
                    publicUrl = upload.publicurl,
                    productImageId = 1
                };

                await _messageBus.PublishMessage(uploadedMessage, productImageuploadedMessageTopic);

                return ResponseHandler<string>.SuccessResponse(SuccessMessages.Successfull, SuccessCodes.DefaultSuccessCode, "");
            }
            else
            {
                ResponseHandler.FailureResponse(ErrorCodes.Invalidorincorrectvaluesprovided, ErrorMessages.Invalidorincorrectvaluesprovided);
            }
        }
        else
        {
            ResponseHandler.FailureResponse(ErrorCodes.Invalidorincorrectvaluesprovided, ErrorMessages.Invalidorincorrectvaluesprovided);
        }

        return ResponseHandler<string>.SuccessResponse(SuccessMessages.Successfull, SuccessCodes.DefaultSuccessCode, "");

    }

    public async Task<ApiResponse<string>> RemoveProductImags(int productImageId, CancellationToken cancellationToken)
    {
        if (productImageId > 0)
        {
            var productdetail = await _productImagesRepository.GetByIdAsync(productImageId, cancellationToken);

            if (productdetail != null)
            {
                

                //Getting blobStorage from Configuratn microservice
                var uploadPath = SuccessCodes.uploadPaths;

                var upload = _azureBlobStorage.DeleteBlobUpload( uploadPath, productdetail.CloudPath);

                if (productdetail.IsPrimaryImage == true)
                {
                    var productimgs = _productImagesRepository.Table.Where(p => p.Id == productImageId
                                                               && p.IsPrimaryImage == false).OrderByDescending(pi=>pi.DateCreated);
                    var newleadImage = productimgs.First();

                     newleadImage.IsPrimaryImage = true;
                     _productImagesRepository.Update(newleadImage);                   

                }


                 _productImagesRepository.Delete(productdetail);


                //Azure ServiceBus
                ProductDeletedMessage deletedMessage = new()
                {
                    dateDeleted = DateTime.UtcNow,
                    deletedByUserId = 1,
                    dateCreated = DateTime.UtcNow, 
                    productImageId = productImageId
                };

                await _messageBus.PublishMessage(deletedMessage, productDeletedMessageTopic);

                return ResponseHandler<string>.SuccessResponse(SuccessMessages.Successfull, SuccessCodes.DefaultSuccessCode, "");
            }
            else
            {
                ResponseHandler.FailureResponse(ErrorCodes.Invalidorincorrectvaluesprovided, ErrorMessages.Invalidorincorrectvaluesprovided);
            }
        }
        else
        {
            ResponseHandler.FailureResponse(ErrorCodes.Invalidorincorrectvaluesprovided, ErrorMessages.Invalidorincorrectvaluesprovided);
        }

        return ResponseHandler<string>.SuccessResponse(SuccessMessages.Successfull, SuccessCodes.DefaultSuccessCode, "");

    }


}
