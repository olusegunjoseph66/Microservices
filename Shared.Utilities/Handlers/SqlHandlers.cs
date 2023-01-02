using Shared.Utilities.DTO;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Shared.Utilities.Handlers
{
    public class SqlHandlers
    {
        public static DuplicateKeyExceptionDto? UniqueErrorFormatter(SqlException ex, bool isMaskingAllowed = false)
        {
            var message = ex.Errors[0].Message;
            if (!string.IsNullOrEmpty(message))
            {
                var matchCompareExpression1 = @"\ACannot insert duplicate key row in object \'(?<TableName>.+?)\' with unique index \'(?<IndexName>.+?)\'\. The duplicate key value is \((?<KeyValues>.+?)\)";
                // var matchCompareExpression2 = @"\AViolation of UNIQUE KEY constraint \'(?<IndexName>.+?)\'. Cannot insert duplicate key in object \'(?<TableName>?)\'\. The duplicate key value is \((?<KeyValues>.+?)\)";
                var matchCompareExpression2 = @"\AViolation of UNIQUE KEY constraint \'(?<IndexName>.+?)\'\. Cannot insert duplicate key in object \'(?<TableName>.+?)\'. The duplicate key value is \((?<KeyValues>.+?)\).";
                var firstMatch = new Regex(matchCompareExpression1, RegexOptions.Compiled).Match(ex.Message);
                var secondMatch = new Regex(matchCompareExpression2, RegexOptions.Compiled).Match(ex.Message);
                Match match;
                if (firstMatch.Captures.Count == 0)
                {
                    match = secondMatch;
                }
                else
                {
                    match = firstMatch;
                }

                string table = match.Groups["TableName"].Value.Replace("dbo.", "");
                string index = match.Groups["IndexName"].Value;
                string value = match.Groups["KeyValues"].Value;
                string[] indexSubStrings = index.Split('_');
                string columnName = indexSubStrings[1];
                DuplicateKeyExceptionDto validationResponse = new()
                {
                    TableName = table,
                    ColumnName = columnName,
                    MaskedColumnValue = isMaskingAllowed ? StringOperations.MaskString(value, 60) : value,
                };
                return validationResponse;
            }

            return null;
        }
    }
}
