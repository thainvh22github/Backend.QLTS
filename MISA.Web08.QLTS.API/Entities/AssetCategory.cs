namespace MISA.Web08.QLTS.API.Entities
{
    public class AssetCategory
    {
        /// <summary>
        /// Id loại tài sản
        /// </summary>
        public Guid CategoryAssetID { get; set; }

        /// <summary>
        /// mã loại tài sản
        /// </summary>
        public string CategoryAssetCode { get; set; }

        /// <summary>
        /// tên loại tài sản
        /// </summary>
        public string CategoryAssetName { get; set; }



    }
}
