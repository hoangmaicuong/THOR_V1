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
using System.Linq;
using System.Text;

namespace THOR_V1.Module.BusinessObjects
{
    [System.ComponentModel.DefaultProperty("InvoiceNumber")]
    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Customer Order Entry")]
    public class PackingList : BaseObject
    {
        #region Header Properties
        // Header section properties as shown in the image
        private String shipper;
        private String billTo;
        private String shipTo;
        private DateTime invoiceDate;      // "XX/XX/2025"
        private String invoiceNum;      // "XX-XX XXX"
        private DateTime loadingDate;
        private String shippingTerms;
        private DateTime deliveryDate;     // "28-Mar-2025"
                                           //private String origin;
        private Customer custometID;  // Ship to address   

        private POTerm pOTermID;
        private PaymentTerm paymentTermID;
        private String origin;
        private Customer customerID;
        #endregion

        public PackingList(Session session) : base(session) { }


        [RuleRequiredField("PackingList Customer is required", DefaultContexts.Save, "Customer is required")]
        [Association("PackingList-Customer", typeof(Customer)), ImmediatePostData]
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

        [Size(255)]

        public String Shipper
        {
            get { return shipper; }
            set { shipper = value; }
        }

        // Do Filter by Customer
        private CustomerAddress shipToID;
        [Association("PackingList-CustomerAddress", typeof(CustomerAddress))]
        public CustomerAddress ShipToID
        {
            get { return shipToID; }
            set { shipToID = value; }
        }

        private CustomerAddress billToID;
        // Do Filter by Customer
        //[Association("PackingList-CustomerAddress", typeof(CustomerAddress))]
        public CustomerAddress BillToID
        {
            get { return billToID; }
            set { billToID = value; }
        }
        [Size(255)]
        public String BillTo
        {
            get
            {
                if (!IsSaving && !IsLoading && !IsDeleted)
                {
                    if (CustomerID != null)
                    {
                        StringBuilder s = new StringBuilder();
                        s.AppendLine(CustomerID.Company);
                        s.AppendLine(CustomerID.Address);
                        billTo = s.ToString();
                    }
                }
                return billTo;
            }
            set { billTo = value; }
        }
        [Size(255)]
        public String ShipTo
        {
            get
            {
                if (!IsSaving && !IsLoading && !IsDeleted)
                {
                    if (CustomerID != null)
                    {
                        StringBuilder s = new StringBuilder();
                        s.AppendLine(CustomerID.Company);
                        s.AppendLine(CustomerID.Address);
                        shipTo = s.ToString();
                    }
                }
                return shipTo;
            }
            set { shipTo = value; }
        }

        public DateTime InvoiceDate
        {
            get { return invoiceDate; }
            set { invoiceDate = value; }
        }

        [Size(50)]

        public DateTime LoadingDate
        {
            get { return loadingDate; }
            set { loadingDate = value; }
        }

        public String InvoiceNum
        {
            get { return invoiceNum; }
            set { invoiceNum = value; }
        }

        public PaymentTerm PaymentTermID
        {
            get { return paymentTermID; }
            set { paymentTermID = value; }
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


        [Association("PackingListDetail-PackingList")]
        public XPCollection<PackingListDetail> PackingListDetails
        {
            get
            {
                return GetCollection<PackingListDetail>("PackingListDetails");
            }
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        protected override void OnDeleting()
        {
            if (Session.CollectReferencingObjects(this).Count > 0)
            {
                throw new InvalidOperationException("The object cannot be deleted. Other objects have references to it.");
            }
            base.OnDeleting();
        }
    }
    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Customer Order Entry")]
    public class PackingListDetail : BaseObject
    {
        #region Details Properties
        // Details section properties as shown in the image
        private PackingList packingList;

        private Item itemId;            // "ITEM#" column
        private Customer cusId;        // "CUST CODE#" column
                                       // "DESCRIPTION" column
        private String hsCode;            // "HS CODE" column
        private int? qtyPcs;               // "QTY (PCS)" column
        private int? qtyCarton;            // "QTY(CARTON)" column
        private Double? unitCbm;           // "UNIT CBM" column
        //private Double totalCbm;          // "TOTAL CBM" column
        private Double nw;                // "NW" column
        private Double totalNw;           // "TOTAL NW" column
        private Double gw;                // "GW" column
        private ItemPack itempack;
        private Double totalGw;           // "TOTAL GW" column
        private PO poId;                // "PO#" column
                                        // Part of "KÍCH THƯỚC HÀNG (MM)" - Length
                                        // Part of "KÍCH THƯỚC HÀNG (MM)" - Width
                                        // Part of "KÍCH THƯỚC HÀNG (MM)" - Height
        private String moTaHangHoa;       // "Mô tả hàng hóa" column
        #endregion

        public PackingListDetail(Session session)
            : base(session)
        {
        }

        #region Details Property Definitions
        [Association("PackingListDetail-PackingList")]
        public PackingList PackingList
        {
            get { return packingList; }
            set { packingList = value; }
        }

        public ItemPack Itempack
        {
            get { return itempack; }
            set { itempack = value; }
        }


        [Association("PackingListDetail-Item", typeof(Item))] //ok
        public Item ItemID
        {
            get { return itemId; }
            set { itemId = value; }
        }
        [Association("PackingListDetail-Customer", typeof(Item))] //ok
        public Customer CustCodeID
        {
            get { return cusId; }
            set { cusId = value; }
        }
        [Size(50)]
        public String HsCode
        {
            get { return hsCode; }
            set { hsCode = value; }
        }

        [RuleRequiredField("Quantity in pieces is required", DefaultContexts.Save)]
        //[RuleCriteria("Quantity must be greater than 0", DefaultContexts.Save, "QtyPcs > 0")]
        public int? QtyPcs
        {
            get { return qtyPcs; }
            set { qtyPcs = value; }
        }

        [RuleRequiredField("Quantity in cartons is required", DefaultContexts.Save)]
        public int? QtyCarton
        {
            get { return qtyCarton; }
            set { qtyCarton = value; }
        }

        [RuleRequiredField("Unit CBM is required", DefaultContexts.Save)]
        [Custom("EditMask", "###,###,###.00")]
        public Double? UnitCbm
        {
            get { return unitCbm; }
            set { unitCbm = value; }
        }

        [Custom("EditMask", "###,###,###.00")]
        [PersistentAlias("UnitCbm * QtyCarton")]
        public Double TotalCbm
        {
            get { return EvaluateAlias("TotalCbm") as Double? ?? 0; }
        }

        [Custom("EditMask", "###,###,###.00")]
        public Double Nw
        {
            get { return nw; }
            set { nw = value; }
        }

        [Custom("EditMask", "###,###,###.00")]
        [PersistentAlias("Nw * qtyCarton")]
        public Double TotalNw
        {
            get { return EvaluateAlias("TotalNw") as Double? ?? 0; }
        }

        [Custom("EditMask", "###,###,###.00")]
        public Double Gw
        {
            get { return gw; }
            set { gw = value; }
        }

        [Custom("EditMask", "###,###,###.00")]
        [PersistentAlias("Gw * qtyCarton")]
        public Double TotalGw
        {
            get { return EvaluateAlias("TotalGw") as Double? ?? 0; }
        }

        public PO PO
        {
            get { return poId; }
            set { poId = value; }
        }

        [Size(255)]
        public String MoTaHangHoa
        {
            get { return moTaHangHoa; }
            set { moTaHangHoa = value; }
        }
        #endregion

        protected override void OnSaving()
        {
            base.OnSaving();
        }
    }
}