using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THOR_V1.Module.BusinessObjects
{
    [Appearance("LockShipTo", AppearanceItemType = "ViewItem", TargetItems = "ShipTo", Context = "Any", Enabled = false)]
    [System.ComponentModel.DefaultProperty("InvoiceNum")]
    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Customer Order Entry")]
    public class CommercialInvoice : BaseObject
    {
        private String shipper;
        private String billTo;
        private String shipTo;
        private DateTime invoiceDate;
        private POTerm pOTermID;
        private PaymentTerm paymentTermID;
        private DateTime deliveryDate;
        private String shippingTerms;
        private String origin;
        private String invoiceNum;
        private DateTime loadingDate;
        private String paymentTerms;
        private Customer customerID;

        public CommercialInvoice(Session session) : base(session) { }

        public PO POID { get; set; }

        public Customer CustomerID
        {
            get { return customerID; }
            set
            {
                if (customerID == value) return;
                customerID = value;
                OnChanged("CustomerID");
            }
        }

        [Size(SizeAttribute.Unlimited)]
        public String Shipper
        {
            get { return shipper; }
            set { shipper = value; }
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        public DateTime InvoiceDate
        {
            get { return invoiceDate; }
            set { invoiceDate = value; }
        }

        public DateTime LoadingDate
        {
            get { return loadingDate; }
            set { loadingDate = value; }
        }

        [Size(100)]
        public String InvoiceNum
        {
            get { return invoiceNum; }
            set { invoiceNum = value; }
        }


        public DateTime DeliveryDate
        {
            get { return deliveryDate; }
            set { deliveryDate = value; }
        }

        public String ShippingTerms
        {
            get { return shippingTerms; }
            set { shippingTerms = value; }
        }

        [Size(100)]
        public String Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        [Size(255)]
        public String BillTo
        {
            get
            {
                //if (!IsSaving && !IsLoading && !IsDeleted)
                //{
                //    if (CustomerID != null)
                //    {
                //        StringBuilder s = new StringBuilder();
                //        s.AppendLine(CustomerID.Company);
                //        s.AppendLine(CustomerID.Address);
                //        billTo = s.ToString();
                //    }
                //}
                return billTo;
            }
            set { billTo = value; }
        }

        [Size(255)]
        public String ShipTo
        {
            get
            {
                //if (!IsSaving && !IsLoading && !IsDeleted)
                //{
                //    if (CustomerID != null)
                //    {
                //        StringBuilder s = new StringBuilder();
                //        s.AppendLine(CustomerID.Company);
                //        s.AppendLine(CustomerID.Address);
                //        shipTo = s.ToString();
                //    }
                //}
                return shipTo;
            }
            set { shipTo = value; }
        }


        // Sử dụng XPCollection<T> thay vì XPCollection
        [Association("CommercialInvoiceDetail-CommercialInvoice", typeof(CommercialInvoiceDetail))]
        public XPCollection<CommercialInvoiceDetail> CommercialInvoiceDetails
        {
            get { return GetCollection<CommercialInvoiceDetail>("CommercialInvoiceDetails"); }
        }

        // Tính tổng tiền - Sử dụng calculation thay vì PersistentAlias
        public Double TotalAmount
        {
            get
            {
                if (IsLoading || IsSaving || IsDeleted)
                    return 0;

                return CommercialInvoiceDetails?.Sum(x => x.Total) ?? 0;
            }
        }

        // Tính tổng số lượng - Sử dụng calculation thay vì PersistentAlias
        public int TotalQty
        {
            get
            {
                if (IsLoading || IsSaving || IsDeleted)
                    return 0;

                return CommercialInvoiceDetails?.Sum(x => x.Qty) ?? 0;
            }
        }

        protected override void OnDeleting()
        {
            foreach (XPMemberInfo mi in ClassInfo.CollectionProperties)
            {
                if (mi.IsCollection && mi.IsAssociation)
                {
                    if (Session.CollectReferencingObjects(this).Count > 0)
                    {
                        foreach (IXPObject obj in Session.CollectReferencingObjects(this))
                        {
                            throw new InvalidOperationException("The object cannot be deleted. Other objects have references to it.");
                        }
                    }
                }
            }
            base.OnDeleting();
        }
    }

    [RuleCriteria("CommercialInvoice PO Qty is greater than 0", DefaultContexts.Save, "Qty > 0", "CommercialInvoice PO Qty is greater than 0", SkipNullOrEmptyValues = false)]
    [System.ComponentModel.DefaultProperty("UniqueName")]
    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Customer Order Entry")]
    public class CommercialInvoiceDetail : BaseObject
    {
        private CommercialInvoice commercialInvoiceID;
        private Item itemId;
        private Customer cusID;
        private int qty;
        private Double unitPrice;
        private String custCode;
        public PO POID { get; set; }

        public CommercialInvoiceDetail(Session session) : base(session) { }

        [Association("CommercialInvoiceDetail-CommercialInvoice", typeof(CommercialInvoice))]
        public CommercialInvoice CommercialInvoiceID
        {
            get { return commercialInvoiceID; }
            set { commercialInvoiceID = value; }
        }

        [Association("CommercialInvoiceDetail-Item", typeof(Item))]
        public Item ItemId
        {
            get { return itemId; }
            set { itemId = value; }
        }

        public int Qty
        {
            get { return qty; }
            set
            {
                qty = value;
                OnChanged("Qty");
            }
        }

        public string CustCode
        {
            get { return custCode; }
            set { custCode = value; }
        }

        [Custom("EditMask", "###,###,###.00")]
        [Persistent]
        public Double UnitPrice
        {
            get { return Math.Round(unitPrice, 2); }
            set
            {
                unitPrice = value;
                OnChanged("UnitPrice");
            }
        }
        private double total;
        [Persistent]
        public Double Total
        {
            get
            {
                // Luôn tính lại nếu chưa có giá trị hoặc đang không trong quá trình save/load
                if (!IsSaving && !IsLoading && !IsDeleted)
                {
                    total = Math.Round(Qty * UnitPrice, 2);
                }
                return total;
            }
            set
            {
                total = value;
                OnChanged("Total");
            }
        }

        [Persistent]
        [RuleUniqueValue("", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        public String UniqueName
        {
            get
            {
                if (CommercialInvoiceID == null)
                    return ItemId == null ? "" : ItemId.ToString();
                else
                    return CommercialInvoiceID.ToString() + '-' + (ItemId == null ? "" : ItemId.ToString());
            }
        }

        protected override void OnDeleting()
        {
            foreach (XPMemberInfo mi in ClassInfo.CollectionProperties)
            {
                if (mi.IsCollection && mi.IsAssociation)
                {
                    if (Session.CollectReferencingObjects(this).Count > 0)
                    {
                        foreach (IXPObject obj in Session.CollectReferencingObjects(this))
                        {
                            throw new InvalidOperationException("The object cannot be deleted. Other objects have references to it.");
                        }
                    }
                }
            }
            base.OnDeleting();
        }

        protected override void OnSaving()
        {
            base.OnSaving();
        }
    }

}
