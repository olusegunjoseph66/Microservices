namespace Shared.Utilities.DTO
{
    public class DuplicateKeyExceptionDto
    {
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string MaskedColumnValue { get; set; }
    }
}
