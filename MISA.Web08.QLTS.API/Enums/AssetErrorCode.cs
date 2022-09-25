namespace MISA.Web08.QLTS.API.Enums
{
    public enum AssetErrorCode
    {

        /// <summary>
        /// Lỗi do exception
        /// </summary>
        Exception = 1,

        /// <summary>
        /// Lỗi do trùng mã
        /// </summary>
        DuplicateCode = 2,

        /// <summary>
        /// Lỗi do nhập thiếu thông tin
        /// </summary>
        EmptyCode = 3,

        /// <summary>
        /// Gọi vào DB để select thất bại
        /// </summary>
        SelectFailed = 4,

        /// <summary>
        /// Gọi vào DB để insert thất bại
        /// </summary>
        InsertFailed = 5,

        /// <summary>
        /// Gọi vào DB để update thất bại
        /// </summary>
        UpdateFailed = 6,

        /// <summary>
        /// Gọi vào DB để delete thất bại
        /// </summary>
        DeleteFailed = 7,

        /// <summary>
        /// Dữ liệu đầu vào không hợp lệ
        /// </summary>
        InvalidInput = 8,
    }
}
