using MISA.Web08.QLTS.API.Enums;

namespace MISA.Web08.QLTS.API.Entities
{
    /// <summary>
    /// Tài sản
    /// </summary>
    public class Asset
    {
        /// <summary>
        /// ID tài sản
        /// </summary>
        public Guid AssetID { get; set; }

        /// <summary>
        /// Mã tài sản
        /// </summary>
        public string AssetName { get; set; }

        public string AssetCode { get; set; }

        public Gender Gender { get; set; }
    }
}
