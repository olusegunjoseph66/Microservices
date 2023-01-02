namespace Wallet.Application.Constants
{
    public class ErrorCodes
    {
        public const string DEFAULT_VALIDATION_CODE = "Error-P-03";
        public const string DEFAULT_AUTHORIZATION_CODE = "E02";
        public const string DATABASE_INSERT_CONFLICT_CODE = "E03";
        public const string SERVER_ERROR_CODE = "E04";
        public const string NOTFOUND_ERROR_CODE = "E05";

        public const int SqlServerViolationOfUniqueIndex = 2601;
        public const int SqlServerViolationOfUniqueConstraint = 2627;
        public const string INVALID_OR_INCORRECT_VALUES = "S02";
        public const string CAN_NOT_UPDATE_WITH_NAME_OF_ANOTHER_CATEGORY = "S04";
        public const string FAQ_DETAILS_NOT_RETRIEVED = "S06";
        public const string CAN_NOT_UPDATE_WITH_QUESTION_OF_ANOTHER_FAQ = "S04";
        public const string Error_W_01= "Error-W-01";
        public const string Error_W_02= "Error-W-02";
    }
}
