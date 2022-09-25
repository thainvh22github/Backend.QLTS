namespace MISA.Web08.QLTS.API.Attributes
{

    /// <summary>
    /// Attribute dùng để xác định 1 property là khóa chính 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKeyAttribute : Attribute
    {

    }

    /// <summary>
    /// Attribure dùng để xác định 1 property không được để trống
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IsNotNullOrEmptyAttribute : Attribute
    {
        #region Field

        /// <summary>
        /// Message lỗi trả về cho client
        /// </summary>
        public string ErrorMessage;

        #endregion

        #region Constructor

        public IsNotNullOrEmptyAttribute(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        #endregion
    }

}
