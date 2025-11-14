using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text;

namespace THOR_V1.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem(GroupName = "Master Lists")]
    [ImageName("Demo_SalesOverview")]
    public class ItemStock : BaseObject
    { 
        public ItemStock(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        private Item _item;
        [XafDisplayName("Item Code")]
        [ModelDefault("Index", "0")]
        [ImmediatePostData] // để cập nhật thuộc tính phụ khi chọn Item
        public Item ItemID
        {
            get => _item;
            set
            {
                if (_item == value)
                    return;

                var prevItem = _item;
                _item = value;

                if (IsLoading)
                    return;

                // Hủy liên kết cũ
                if (prevItem != null && prevItem.ItemStockID == this)
                    prevItem.ItemStockID = null;

                // Gán liên kết mới
                if (_item != null)
                    _item.ItemStockID = this;

                OnChanged(nameof(ItemID));
            }
        }
        [ModelDefault("Index", "1")]
        [XafDisplayName("Description")]
        public string ItemDescription => ItemID?.Description;

        [NonPersistent]
        [ImageEditor(
            ListViewImageEditorMode = ImageEditorMode.PictureEdit,
            DetailViewImageEditorMode = ImageEditorMode.PictureEdit,
            ListViewImageEditorCustomHeight = 60)]
        [XafDisplayName("Item Image")]
        public byte[] ItemImage
        {
            get
            {
                if (ItemID?.IImage == null)
                    return null;

                using (var ms = new MemoryStream())
                {
                    ItemID.IImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    return ms.ToArray();
                }
            }
        }

        private int _BOMQty;
        public int BOMQty
        {
            get => _BOMQty;
            set => SetPropertyValue(nameof(BOMQty), ref _BOMQty, value);
        }

        private DateTime? _purDate;
        public DateTime? PurDate
        {
            get => _purDate;
            set => SetPropertyValue(nameof(PurDate), ref _purDate, value);
        }

        private string _suplier;
        public string Suplier
        {
            get => _suplier;
            set => SetPropertyValue(nameof(Suplier), ref _suplier, value);
        }

        private int _stockQty;
        public int StockQty
        {
            get => _stockQty;
            set => SetPropertyValue(nameof(StockQty), ref _stockQty, value);
        }

        private int _totalReceipt;
        public int TotalReceipt
        {
            get => _totalReceipt;
            set => SetPropertyValue(nameof(TotalReceipt), ref _totalReceipt, value);
        }

        private int _totalIssue;
        public int TotalIssue
        {
            get => _totalIssue;
            set => SetPropertyValue(nameof(TotalIssue), ref _totalIssue, value);
        }

        private int _priceAVG;
        public int PriceAVG
        {
            get => _priceAVG;
            set => SetPropertyValue(nameof(PriceAVG), ref _priceAVG, value);
        }

        // ✅ Quan hệ 1-n: Một ItemStock có nhiều PurchasingHistory
        [DevExpress.Xpo.Association("ItemStock-PurchasingHistories")]
        public XPCollection<PurchasingHistory> PurchasingHistories
        {
            get { return GetCollection<PurchasingHistory>(nameof(PurchasingHistories)); }
        }
    }
}