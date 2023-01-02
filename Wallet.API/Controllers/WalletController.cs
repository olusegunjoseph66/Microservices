using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Wallet.Application.Interfaces.Services;
using Wallet.Application.ViewModels.Responses;

namespace Wallet.API.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;
        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpGet("{distributorSapAccountId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SapWalletResponse))]
        public async Task<ActionResult<SapWalletResponse>> GetMySAPWallet(int distributorSapAccountId,bool forceRefresh, CancellationToken cancellationToken = default)
        {
            return Ok(await _walletService.GetMySAPWallet(distributorSapAccountId,forceRefresh, cancellationToken));
        }


        [HttpGet("{distributorSapAccountId}/statement/from/{fromDate}/to/{toDate}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SapWalletStatementResponse))]
        public async Task<ActionResult<SapWalletStatementResponse>> GetMySAPWalletStatement([Required]int distributorSapAccountId, [Required] DateTime fromDate, DateTime? toDate=null, CancellationToken cancellationToken = default)
        {
            return Ok(await _walletService.GetMySAPWalletStatement(distributorSapAccountId,fromDate,toDate, cancellationToken));
        }

    }
}
