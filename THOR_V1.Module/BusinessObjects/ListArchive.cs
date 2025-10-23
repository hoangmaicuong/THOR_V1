using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.XtraRichEdit.API.Native;
using DevExpress.XtraRichEdit.Import.Html;
using DevExpress.XtraRichEdit.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THOR_V1.Module.BusinessObjects
{
    [System.ComponentModel.DefaultProperty("UnitName")]
    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Lists Archive")]
    public class Units : BaseObject
    {
        public Units(Session session) : base(session) { }

        public string UnitName { get; set; }

        public Double ConversionFacto { get; set; }

        [Association("Units-Item", typeof(Item))]
        public XPCollection Items
        {
            get
            {
                return GetCollection("Items");
            }
        }
    }

    [System.ComponentModel.DefaultProperty("BrandName")]
    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Lists Archive")]
    public class BrandList : BaseObject
    {
        private String brandName;
        private String brandItemPrefix;
        public BrandList(Session session) : base(session) { }

        [RuleRequiredField("Brand Name is required", DefaultContexts.Save, "Brand Name is required")]
        [RuleUniqueValue("", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        [Size(50)]
        public String BrandName
        {
            get { return brandName; }
            set { brandName = value; }
        }

        [Size(2)]

        public String BrandItemPrefix
        {
            get { return brandItemPrefix; }
            set { brandItemPrefix = value; }
        }
        [Association("Item-BrandList", typeof(Item))]
        public XPCollection Items
        {
            get { return GetCollection("Items"); }
        }
        protected override void OnDeleting()
        {
            foreach (XPMemberInfo mi in ClassInfo.CollectionProperties)
            {
                if (mi.IsCollection && mi.IsAssociation)  //mi.IsAggregated &&
                {
                    if (Session.CollectReferencingObjects(this).Count > 0)
                    {
                        foreach (IXPObject obj in Session.CollectReferencingObjects(this))
                        {
                            // if (((XPBaseCollection)mi.GetValue(this)).BaseIndexOf(obj) < 0)
                            {
                                throw new InvalidOperationException("The object cannot be deleted. Other objects have references to it.");
                            }
                        }
                    }
                }
            }
            base.OnDeleting();
        }
    }

    [System.ComponentModel.DefaultProperty("ShortClass")]
    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Lists Archive")]
    public class ItemClass : BaseObject //Item_Category
    {
        private String classes;//name;
        private String shortClass;  //shortName
        private String fullClass; //Description
        private String guideLines;

        public ItemClass(Session session)
            : base(session)
        {
        }
        [Size(100)]
        public String Classes
        {
            get { return classes; }
            set { classes = value; }
        }

        [Size(20)]
        [RuleRequiredField("ItemClass ShortClass is required", DefaultContexts.Save, "ShortClass is required")]
        [RuleUniqueValue("", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        public String ShortClass
        {
            get { return shortClass; }
            set { shortClass = value; }
        }

        [Size(100)]
        public String FullClass
        {
            get { return fullClass; }
            set { fullClass = value; }
        }

        [Size(100)]
        public String GuideLines
        {
            get { return guideLines; }
            set { guideLines = value; }
        }
        [Association("Item-ItemClass", typeof(Item))]
        public XPCollection Items
        {
            get
            {
                return GetCollection("Items");
            }
        }
        protected override void OnDeleting()
        {
            foreach (XPMemberInfo mi in ClassInfo.CollectionProperties)
            {
                if (mi.IsCollection && mi.IsAssociation)  //mi.IsAggregated &&
                {
                    if (Session.CollectReferencingObjects(this).Count > 0)
                    {
                        foreach (IXPObject obj in Session.CollectReferencingObjects(this))
                        {
                            // if (((XPBaseCollection)mi.GetValue(this)).BaseIndexOf(obj) < 0)
                            {
                                throw new InvalidOperationException("The object cannot be deleted. Other objects have references to it.");
                            }
                        }
                    }
                }
            }
            base.OnDeleting();
        }
    }

    [RuleCriteria("UnitPrice is greater than 0", DefaultContexts.Save, "UnitPrice > 0", "UnitPrice is greater than 0", SkipNullOrEmptyValues = false)]
    [System.ComponentModel.DefaultProperty("ItemID")]
    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Master Lists")]
    public class ItemPriceHistory : BaseObject
    {
        private Item itemID;
        private Double? unitPrice;
        private DateTime latestApproved;
        private DateTime validFrom;
        private String comments;
        //private Double margin;
        public ItemPriceHistory(Session session)
            : base(session)
        {
        }
        [RuleRequiredField("Item Item UnitPrice is required", DefaultContexts.Save, "Item is Required")]
        [Association("Item-ItemPriceHistory", typeof(Item))]
        public Item ItemID
        {
            get { return itemID; }
            set { itemID = value; }
        }

        [RuleRequiredField("Item Unit Price is required", DefaultContexts.Save, "UnitPrice is required")]
        [Custom("EditMask", "###,###.00")]
        public Double? UnitPrice
        {
            get { return unitPrice; }
            set { unitPrice = value; }
        }
        public DateTime LatestApproved
        {
            get { return latestApproved; }
            set { latestApproved = value; }
        }
        //public Double VariancesHPPrice_DDP
        //{
        //    get
        //    {
        //        if (UnitPrice > 0)
        //            return UnitPrice * 0.25;
        //        else 
        //            return 0;

        //    }
        //}
        [Custom("EditMask", "dd-MM-yyyy")]
        [RuleRequiredField("ItemPriceHistory ValidFrom Date is required", DefaultContexts.Save, "ItemPriceHistory ValidFrom Date  is required")]
        public DateTime ValidFrom
        {
            get { return validFrom; }
            set { validFrom = value; }
        }
        [Size(200)]

        public String Comments
        {
            get { return comments; }
            set { comments = value; }
        }
        [NonPersistent]
        //  [RuleUniqueValue("", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]

        [RuleUniqueValue("RuleUniqueValueValidDateItemPrice", DefaultContexts.Save, "ValidFrom is exists!")]
        public string UniqueNumber
        {
            get
            {
                string uniqueNumber = "";

                if (ItemID != null)
                    uniqueNumber += ItemID.ToString();

                uniqueNumber += (ValidFrom == null) ? "" : ValidFrom.ToString();
                return uniqueNumber;

            }
        }
    }

    [System.ComponentModel.DefaultProperty("ITEMBRKID")]
    [DefaultClassOptions, ImageName("Archive_Large")]
    [NavigationItem(GroupName = "Lists Archive")]
    public class ITEMBRK : BaseObject
    {
        public ITEMBRK(Session session) : base(session) { }

        [Size(20)]
        [RuleUniqueValue("", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        [Indexed]
        public String ITEMBRKID { get; set; }

        [Size(200)]
        public String Description { get; set; }

        [Association("ITEMBRK-Item", typeof(Item))]
        public XPCollection Items
        {
            get
            {
                return GetCollection("Items");
            }
        }



    }

    [System.ComponentModel.DefaultProperty("DEFPRICLSTNO")]
    [DefaultClassOptions, ImageName("Archive_Large")]
    [NavigationItem(GroupName = "Lists Archive")]
    public class DEFPRICLST : BaseObject
    {
        public DEFPRICLST(Session session) : base(session) { }

        [Size(20)]
        [RuleUniqueValue("", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        [Indexed]
        public String DEFPRICLSTNO { get; set; }

        [Size(200)]
        public String Description { get; set; }

        public bool IsActive { get; set; }

        public bool CommissionPaid { get; set; }

        [Association("DEFPRICLST-Item", typeof(Item))]
        public XPCollection Items
        {
            get
            {
                return GetCollection("Items");
            }
        }

    }

    [System.ComponentModel.DefaultProperty("DesignerInfo")]
    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Lists Archive")]
    public class DesignerBy : BaseObject
    {
        private String designerInfo;
        public DesignerBy(Session session) : base(session) { }
        public String DesignerInfo
        {
            get { return designerInfo; }
            set { designerInfo = value; }
        }

        /*[Association("DesignerBy-Item", typeof(Item))]
        public XPCollection Items
        {
            get
            {
                return GetCollection("Items");
            }
        }*/

    }

    [System.ComponentModel.DefaultProperty("CategoryCode")]
    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Lists Archive")]
    public class ItemCategory : BaseObject
    {
        public ItemCategory(Session session) : base(session) { }

        [Size(20)]
        [RuleUniqueValue("", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        [Indexed]
        public String CategoryCode { get; set; }

        [Size(200)]
        public String Description { get; set; }

        public bool IsActive { get; set; }

        public bool CommissionPaid { get; set; }

        [Association("ItemCategory-Item", typeof(Item))]
        public XPCollection Items
        {
            get
            {
                return GetCollection("Items");
            }
        }

    }

    [System.ComponentModel.DefaultProperty("CNTLACCTNO")]
    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Lists Archive")]
    public class CNTLACCT : BaseObject
    {
        public CNTLACCT(Session session) : base(session) { }

        [Size(20)]
        [RuleUniqueValue("", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        [Indexed]
        public String CNTLACCTNO { get; set; }

        [Size(200)]
        public String Description { get; set; }

        public bool IsActive { get; set; }

        public bool CommissionPaid { get; set; }

        [Association("CNTLACCT-Item", typeof(Item))]
        public XPCollection Items
        {
            get
            {
                return GetCollection("Items");
            }
        }

    }


    [System.ComponentModel.DefaultProperty("ContactCode")]
    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Lists Archive")]
    public class ContactType : BaseObject
    {
        private String contactCode; //20
        private String contactName;//100

        public ContactType(Session session)
            : base(session)
        {
        }
        [RuleUniqueValue("", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        [Size(20)]
        public String ContactCode
        {
            get { return contactCode; }
            set { contactCode = value; }
        }
        [Size(100)]
        public String ContactName
        {
            get { return contactName; }
            set { contactName = value; }
        }
        [Association("CustomerAddress-ContactType", typeof(CustomerAddress))]
        public XPCollection CustomerAddresss
        {
            get
            {
                return GetCollection("CustomerAddresss");
            }
        }
    }

    [System.ComponentModel.DefaultProperty("FreightTypeCode")]
    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Lists Archive")]
    public class FreightType : BaseObject
    {

        private String freight;
        private double unitPriceFactor;
        private String discount;

        public FreightType(Session session)
            : base(session)
        {

        }
        public String Freight
        {
            get { return freight; }
            set { freight = value; }
        }
        public String Discount
        {
            get { return discount; }
            set { discount = value; }
        }
        public double UnitPriceFactor
        {
            get { return unitPriceFactor; }
            set { unitPriceFactor = value; }
        }
        [Association("CustomerDiscount-FreightType", typeof(CustomerDiscount))]
        public XPCollection CustomerDiscounts
        {
            get
            {
                return GetCollection("CustomerDiscounts");
            }
        }
        [Association("PO-FreightType", typeof(PO))]
        public XPCollection POs
        {
            get
            {
                return GetCollection("POs");
            }
        }
    }

    [System.ComponentModel.DefaultProperty("DesignerInfo")]
    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Lists Archive")]
    public class Supplier : BaseObject
    {
        private String company;
        private String shortName;
        private String address;
        private String telephone;
        private String fax;
        private String contact;
        private PaymentTerm paymentTermID;
        public Supplier(Session session)
            : base(session)
        {
        }
        [RuleRequiredField("Company Supplier is required", DefaultContexts.Save, "Company is required")]
        [RuleUniqueValue("", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]

        [Size(100)]
        public String Company
        {
            get { return company; }
            set { company = value; }
        }
        [RuleRequiredField("Supplier Short Name is required", DefaultContexts.Save, "ShortName is required")]
        [RuleUniqueValue("", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]

        [Size(50)]
        public String ShortName
        {
            get { return shortName; }
            set { shortName = value; }
        }

        [Size(200)]
        public String Address
        {
            get { return address; }
            set { address = value; }
        }

        [Size(50)]
        public String Telephone
        {
            get { return telephone; }
            set { telephone = value; }
        }

        [Size(50)]
        public String Fax
        {
            get { return fax; }
            set { fax = value; }
        }

        [Size(200)]
        public String Contact
        {
            get { return contact; }
            set { contact = value; }
        }

        [Association("Supplier-PaymentTerm", typeof(PaymentTerm))]
        public PaymentTerm PaymentTermID
        {
            get { return paymentTermID; }
            set { paymentTermID = value; }
        }

        [Association("Tag-Supplier", typeof(Tag))]
        public XPCollection Tags
        {
            get { return GetCollection("Tags"); }
        }

        /*
        [Association("Receipt-Supplier", typeof(Receipt))]
        public XPCollection Receipts
        {
            get { return GetCollection("Receipts"); }
        }

        [Association("SO-Supplier", typeof(SO))]
        public XPCollection SOs
        {
            get
            {
                return GetCollection("SOs");
            }
        }
        */

        protected override void OnDeleting()
        {
            foreach (XPMemberInfo mi in ClassInfo.CollectionProperties)
            {
                if (mi.IsCollection && mi.IsAssociation)  //mi.IsAggregated &&
                {
                    if (Session.CollectReferencingObjects(this).Count > 0)
                    {
                        foreach (IXPObject obj in Session.CollectReferencingObjects(this))
                        {
                            // if (((XPBaseCollection)mi.GetValue(this)).BaseIndexOf(obj) < 0)
                            {
                                throw new InvalidOperationException("The object cannot be deleted. Other objects have references to it.");
                            }
                        }
                    }
                }
            }
            base.OnDeleting();
        }
    }


    [System.ComponentModel.DefaultProperty("Size")]
    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Lists Archive")]
    public class ContainerSize : BaseObject
    {
        private String size;
        private String description;
        private Double maxCBM;

        public ContainerSize(Session session)
            : base(session)
        {
        }
        [RuleUniqueValue("", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        [Size(50)]
        public String Size
        {
            get
            {
                return size;
            }
            set
            {
                if (size == value)
                    return;
                size = value;
            }
        }
        public Double MaxCBM  // 20feet 37CBM  40 feet 58CBM  40HC 65 CBM
        {
            get
            {
                return maxCBM;
            }
            set
            {
                if (maxCBM == value)
                    return;
                maxCBM = value;
            }
        }
        [Size(100)]
        public String Description
        {
            get
            {
                return description;
            }
            set
            {
                if (description == value)
                    return;
                description = value;
            }
        }
        [Association("Shipment-ContainerSize", typeof(Shipment))]
        public XPCollection Shipments
        {
            get
            {
                return GetCollection("Shipments");
            }
        }
        protected override void OnDeleting()
        {
            foreach (XPMemberInfo mi in ClassInfo.CollectionProperties)
            {
                if (mi.IsCollection && mi.IsAssociation) // && mi.IsAggregated)
                {
                    if (Session.CollectReferencingObjects(this).Count > 0)
                    {
                        foreach (IXPObject obj in Session.CollectReferencingObjects(this))
                        {
                            // if (((XPBaseCollection)mi.GetValue(this)).BaseIndexOf(obj) < 0)
                            {
                                throw new InvalidOperationException("The object cannot be deleted. Other objects have references to it.");
                            }
                        }
                    }
                }
            }
            base.OnDeleting();
        }
    }

    [System.ComponentModel.DefaultProperty("Code")]
    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Lists Archive")]
    public class ShipmentTerm : BaseObject
    {
        private String code;
        private String name;

        public ShipmentTerm(Session session)
            : base(session)
        {
        }
        [RuleRequiredField("Shipment Term Code is required", DefaultContexts.Save, "Code is required")]
        [RuleUniqueValue("", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        [Size(50)]
        public String Code
        {
            get { return code; }
            set { code = value; }
        }
        [Size(100)]
        public String Name
        {
            get { return name; }
            set { name = value; }
        }
        [Association("Shipment-ShipmentTerm", typeof(Shipment))]
        public XPCollection Shipments
        {
            get
            {
                return GetCollection("Shipments");
            }
        }

    }
    [System.ComponentModel.DefaultProperty("Code")]
    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Lists Archive")]
    public class InCharge : BaseObject
    {
        private String code;
        private String name;

        public InCharge(Session session)
            : base(session)
        {
        }
        [RuleRequiredField("InCharge Code is required", DefaultContexts.Save, "Code is required")]
        [RuleUniqueValue("", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        [Size(50)]
        public String Code
        {
            get { return code; }
            set { code = value; }
        }
        [Size(100)]
        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        [Association("ProformaInvoice-Incharge", typeof(ProformaInvoice))]
        public XPCollection ProformaInvoices
        {
            get
            {
                return GetCollection("ProformaInvoices");
            }
        }

        [Association("Shipment-InCharge", typeof(Shipment))]
        public XPCollection Shipments
        {
            get
            {
                return GetCollection("Shipments");
            }
        }
    }

    [System.ComponentModel.DefaultProperty("Code")]
    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Lists Archive")]
    public class PaymentTerm : BaseObject
    {
        private String code;
        private String name;

        public PaymentTerm(Session session)
            : base(session)
        {
        }
        [RuleRequiredField("Payment Term Code is required", DefaultContexts.Save, "Code is required")]
        [RuleUniqueValue("", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        [Size(50)]
        public String Code
        {
            get { return code; }
            set { code = value; }
        }
        [Size(100)]
        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public Double ValueFactor { get; set; }

        [Association("Customer-PaymentTerm", typeof(Customer))]
        public XPCollection Customers
        {
            get
            {
                return GetCollection("Customers");
            }
        }

        [Association("ProformaInvoice-PaymentTerm", typeof(ProformaInvoice))]
        public XPCollection ProformaInvoices
        {
            get
            {
                return GetCollection("ProformaInvoices");
            }
        }

        [Association("Supplier-PaymentTerm", typeof(Supplier))]
        public XPCollection Suppliers
        {
            get
            {
                return GetCollection("Suppliers");
            }
        }
    }

    [System.ComponentModel.DefaultProperty("OrderTypeCode")]
    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Lists Archive")]
    public class OrderType : BaseObject
    {
        private String orderTypeCode;
        private String orderTypeName;
        private bool isDirect;

        public OrderType(Session session)
            : base(session)
        {
        }
        [Size(20)]
        public String OrderTypeCode
        {
            get { return orderTypeCode; }
            set { orderTypeCode = value; }
        }
        [Size(100)]
        public String OrderTypeName
        {
            get { return orderTypeName; }
            set { orderTypeName = value; }
        }

        public bool IsDirect
        {
            get { return isDirect; }
            set { isDirect = value; }
        }

        [Association("PO-OrderType", typeof(PO))]
        public XPCollection POs
        {
            get
            {
                return GetCollection("POs");
            }
        }
        [Association("CustomerDiscount-OrderType", typeof(CustomerDiscount))]
        public XPCollection CustomerDiscounts
        {
            get
            {
                return GetCollection("CustomerDiscounts");
            }
        }
        [Association("ProformaInvoice-OrderType", typeof(ProformaInvoice))]
        public XPCollection ProformaInvoices
        {
            get
            {
                return GetCollection("ProformaInvoices");
            }
        }
        protected override void OnDeleting()
        {
            foreach (XPMemberInfo mi in ClassInfo.CollectionProperties)
            {
                if (mi.IsCollection && mi.IsAssociation)  //mi.IsAggregated &&
                {
                    if (Session.CollectReferencingObjects(this).Count > 0)
                    {
                        foreach (IXPObject obj in Session.CollectReferencingObjects(this))
                        {
                            // if (((XPBaseCollection)mi.GetValue(this)).BaseIndexOf(obj) < 0)
                            {
                                throw new InvalidOperationException("The object cannot be deleted. Other objects have references to it.");
                            }
                        }
                    }
                }
            }
            base.OnDeleting();
        }
    }

    [System.ComponentModel.DefaultProperty("CustomerID")]
    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Lists Archive")]
    public class CustomerDiscount : BaseObject
    {
        private Customer customerID;
        private Double discount;
        private FreightType freightTypeID;
        private OrderType orderTypeID;
        private bool defaultDiscount;
        private POTerm pOTermID;
        private DateTime validFrom;

        public CustomerDiscount(Session session)
            : base(session)
        {
        }
        [Association("CustomerDiscount-Customer", typeof(Customer))]
        public Customer CustomerID
        {
            get { return customerID; }
            set { customerID = value; }
        }
        [Association("CustomerDiscount-POTerm", typeof(POTerm))]
        public POTerm POTermID
        {
            get { return pOTermID; }
            set { pOTermID = value; }
        }
        public Double Discount
        {
            get
            {

                if (POTermID != null)
                {
                    discount = POTermID.UnitPriceFactor;
                }
                else
                {
                    discount = 0;
                }
                return discount;
            }
            set { discount = value; }
        }

        [Association("CustomerDiscount-FreightType", typeof(FreightType))]
        public FreightType FreightTypeID
        {
            get { return freightTypeID; }
            set { freightTypeID = value; }
        }
        [Association("CustomerDiscount-OrderType", typeof(OrderType))]
        public OrderType OrderTypeID
        {
            get { return orderTypeID; }
            set { orderTypeID = value; }
        }
        public bool DefaultDiscount
        {
            get { return defaultDiscount; }
            set { defaultDiscount = value; }
        }

        //public DateTime ValidDate { get; set; }
        [RuleRequiredField("Customer Discount Valid From is required", DefaultContexts.Save, "Customer Discount Valid From is required")]
        public DateTime ValidFrom
        {
            get { return validFrom; }
            set { validFrom = value; }
        }
    }

    [System.ComponentModel.DefaultProperty("Display")]
    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Lists Archive")]
    public class POTerm : BaseObject
    {

        private String freight;
        private double unitPriceFactor;
        private String term;
        //private String display;

        public POTerm(Session session)
            : base(session)
        {

        }
        public String Freight
        {
            get { return freight; }
            set { freight = value; }
        }
        public String Term
        {
            get { return term; }
            set { term = value; }
        }
        public double UnitPriceFactor
        {
            get { return unitPriceFactor; }
            set { unitPriceFactor = value; }
        }
        public String Display
        {
            get
            {
                return Freight + " " + UnitPriceFactor;
            }
        }
        [Association("CustomerDiscount-POTerm", typeof(CustomerDiscount))]
        public XPCollection CustomerDiscounts
        {
            get
            {
                return GetCollection("CustomerDiscounts");
            }
        }
        [Association("PO-POTerm", typeof(PO))]
        public XPCollection POs
        {
            get
            {
                return GetCollection("POs");
            }
        }
        [Association("ProformaInvoice-POTerm", typeof(ProformaInvoice))]
        public XPCollection ProformaInvoices
        {
            get
            {
                return GetCollection("ProformaInvoices");
            }
        }
    }

    [System.ComponentModel.DefaultProperty("Address")]
    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Lists Archive")]
    public class CustomerAddress : BaseObject
    {
        private Customer customerID;
        private ContactType contactTypeID;
        private String contactName;
        private String address;
        private String city;
        private String state;
        private String zipCode;
        private String attention;
        private String phone;
        private String handPhone;
        private String fax;
        private String comments;
        private CountryList countryListID;

        public CustomerAddress(Session session)
            : base(session)
        {
        }
        [Association("CustomerAddress-Customer", typeof(Customer))]
        public Customer CustomerID
        {
            get { return customerID; }
            set { customerID = value; }

        }
        [Association("CustomerAddress-CountryList", typeof(CountryList))]
        public CountryList CountryListID
        {
            get
            {
                return countryListID;
            }
            set
            {
                if (countryListID == value)
                    return;
                countryListID = value;
            }
        }
        [RuleRequiredField("CustomerAddress Contact Type is required", DefaultContexts.Save, "Contact Type is required")]
        [Association("CustomerAddress-ContactType", typeof(ContactType))]
        public ContactType ContactTypeID
        {
            get { return contactTypeID; }
            set { contactTypeID = value; }
        }
        [Size(100)]
        // [RuleUniqueValue("", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        public String ContactName
        {
            get { return contactName; }
            set { contactName = value; }
        }
        public override void AfterConstruction()  //Create New //
        {
            base.AfterConstruction();
            //DetailView dv = (DevExpress.ExpressApp.DetailView) DetailViewItem.View ;
            //DetailViewItem item = dv.FindItem("CustomerID");
            //Customer p = (Customer)item.CurrentObject;

            if (CustomerID != null)
            {
                Address = CustomerID.Address;  //p.Address
                ContactName = CustomerID.Contact;   //p.Contact
            }

        }
        //[Persistent]
        //[RuleUniqueValue("", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        //public String UniqueName
        //{
        //    get
        //    {
        //        if (ContactTypeID == null)
        //            return ContactName == null ? "" : ContactName;
        //        else
        //            return ContactTypeID.ContactName + '-' + ContactName;
        //    }

        //}
        [Size(500)]
        public String Address
        {
            get { return address; }
            set { address = value; }
        }
        [Size(20)]
        public String City
        {
            get { return city; }
            set { city = value; }
        }
        [Size(20)]
        public String State
        {
            get { return state; }
            set { state = value; }
        }
        [Size(50)]
        public String ZipCode
        {
            get { return zipCode; }
            set { zipCode = value; }
        }
        [Size(100)]
        public String Attention
        {
            get { return attention; }
            set { attention = value; }
        }
        [Size(50)]
        public String Phone
        {
            get { return phone; }
            set { phone = value; }
        }
        [Size(50)]

        public String HandPhone
        {
            get { return handPhone; }
            set { handPhone = value; }
        }

        [Size(50)]
        public String Fax
        {
            get { return fax; }
            set { fax = value; }
        }
        [Size(255)]

        public String Comments
        {
            get { return comments; }
            set { comments = value; }
        }
        //thêm vào ListArchive.cs chỗ function CustomerAdress

        [Association("PackingList-CustomerAddress", typeof(PackingList))]
        public XPCollection<PackingList> PackingLists
        {
            get { return GetCollection<PackingList>(nameof(PackingLists)); }
        }


        [Association("ShipmentShipTo-CustomerAddress", typeof(Shipment))]
        public XPCollection ShipmentShipTo
        {
            get
            {
                return GetCollection("ShipmentShipTo");
            }
        }

        [Association("ShipmentBillTo-CustomerAddress", typeof(Shipment))]
        public XPCollection ShipmentBillTo
        {
            get
            {
                return GetCollection("ShipmentBillTo");
            }
        }
    }

    [System.ComponentModel.DefaultProperty("CountryCode")]
    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Lists Archive")]
    public class CountryList : BaseObject
    {
        private String countryCode;
        private String countryName;
        public CountryList(Session session)
            : base(session)
        {
        }
        [Size(20)]
        public String CountryCode
        {
            get { return countryCode; }
            set { countryCode = value; }
        }
        [Size(100)]
        public String CountryName
        {
            get { return countryName; }
            set { countryName = value; }
        }
        [Association("Customer-CountryList", typeof(Customer))]
        public XPCollection Customers
        {
            get
            {
                return GetCollection("Customers");
            }
        }
        [Association("CustomerAddress-CountryList", typeof(CustomerAddress))]
        public XPCollection CustomerAddresss
        {
            get
            {
                return GetCollection("CustomerAddresss");
            }
        }
        protected override void OnDeleting()
        {
            foreach (XPMemberInfo mi in ClassInfo.CollectionProperties)
            {
                if (mi.IsCollection && mi.IsAssociation)  //mi.IsAggregated &&
                {
                    if (Session.CollectReferencingObjects(this).Count > 0)
                    {
                        foreach (IXPObject obj in Session.CollectReferencingObjects(this))
                        {
                            // if (((XPBaseCollection)mi.GetValue(this)).BaseIndexOf(obj) < 0)
                            {
                                throw new InvalidOperationException("The object cannot be deleted. Other objects have references to it.");
                            }
                        }
                    }
                }
            }
            base.OnDeleting();
        }
        //[Association("Provider-CountryList", typeof(Provider))]
        //public XPCollection Providers
        //{
        //    get
        //    {
        //        return GetCollection("Providers");
        //    }
        //}
    }

    [System.ComponentModel.DefaultProperty("ContinentCode")]
    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Lists Archive")]
    public class Continent : BaseObject
    {
        private String continentCode;
        private String continentName;
        public Continent(Session session)
            : base(session)
        {
        }
        [Size(20)]
        public String ContinentCode
        {
            get { return continentCode; }
            set { continentCode = value; }
        }
        [Size(100)]
        public String ContinentName
        {
            get { return continentName; }
            set { continentName = value; }
        }
        [Association("Customer-Continent", typeof(Customer))]
        public XPCollection Customers
        {
            get
            {
                return GetCollection("Customers");
            }
        }

    }

    [System.ComponentModel.DefaultProperty("ShortName")]
    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Customer Order Entry")]
    public class Customer : BaseObject
    {
        private String company;
        private String shortName;
        private String address;
        private CountryList countryID;
        private Continent continentID;
        private String contact;
        private PaymentTerm paymentTermID;
        private String comments;//1000
        private bool isActiveCust;
        private double discount;
        public Customer(Session session)
            : base(session)
        {
        }
        [RuleRequiredField("Customer Company Name is required", DefaultContexts.Save, "Company is required")]
        [RuleUniqueValue("", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]

        [Size(100)]
        public String Company
        {
            get { return company; }
            set { company = value; }
        }
        [RuleRequiredField("Customer Short Name is required", DefaultContexts.Save, "Shortname is required")]
        [RuleUniqueValue("", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]

        [Size(20)]
        public String ShortName
        {
            get { return shortName; }
            set { shortName = value; }
        }
        [Size(300)]
        public String Address
        {
            get { return address; }
            set { address = value; }
        }

        [RuleRequiredField("Customer Country is required ", DefaultContexts.Save, "Country is required")]
        [Association("Customer-CountryList", typeof(CountryList))]
        public CountryList CountryID
        {
            get { return countryID; }
            set { countryID = value; }
        }
        [Association("Customer-Continent", typeof(Continent))]
        public Continent ContinentID
        {
            get
            {
                return continentID;
            }
            set
            {
                if (continentID == value)
                    return;
                continentID = value;
            }
        }

        [Size(200)]
        public String Contact
        {
            get { return contact; }
            set { contact = value; }
        }

        [RuleRequiredField("InChargeID is required ", DefaultContexts.Save, "InChargeID is required")]
        public InCharge InChargeID { get; set; }

        [RuleRequiredField("Customer Payment Term is required ", DefaultContexts.Save, "Payment Term is required")]
        [Association("Customer-PaymentTerm", typeof(PaymentTerm))]
        public PaymentTerm PaymentTermID
        {
            get { return paymentTermID; }
            set { paymentTermID = value; }
        }
        [Size(500)]
        public String Comments
        {
            get { return comments; }
            set { comments = value; }
        }
        public bool IsActiveCust
        {
            get { return isActiveCust; }
            set { isActiveCust = value; }
        }

        public Double Discount
        {
            get { return discount; }
            set { discount = value; }
        }

        [Association("FinishList-Customer", typeof(FinishList))]
        public XPCollection FinishLists
        {
            get
            {
                return GetCollection("FinishLists");
            }
        }

        [Association("CustomerDiscount-Customer", typeof(CustomerDiscount))]
        public XPCollection CustomerDiscounts
        {
            get
            {
                return GetCollection("CustomerDiscounts");
            }
        }

        [Association("CustomerAddress-Customer", typeof(CustomerAddress))]
        public XPCollection CustomerAddresss
        {
            get
            {
                return GetCollection("CustomerAddresss");
            }
        }

        [Association("PO-Customer", typeof(PO))]
        public XPCollection POs
        {
            get
            {
                return GetCollection("POs");
            }
        }

        [Association("Invoice-Customer", typeof(Invoice))]
        public XPCollection Invoices
        {
            get
            {
                return GetCollection("Invoices");
            }
        }

        [Association("ProformaInvoice-Customer", typeof(ProformaInvoice))]
        public XPCollection ProformaInvoices
        {
            get
            {
                return GetCollection("ProformaInvoices");
            }
        }
        [Association("Shipment-Customer", typeof(Shipment))]  //ok
        public XPCollection Shipments
        {
            get
            {
                return GetCollection("Shipments");
            }
        }

        //thêm vào ListArchive.cs chỗ function Customer 



        [Association("PackingList-Customer", typeof(PackingList))]
        public XPCollection<PackingList> PackingLists
        {
            get { return GetCollection<PackingList>(nameof(PackingLists)); }
        }

        [Association("PackingListDetail-Customer", typeof(PackingListDetail))]
        public XPCollection<PackingListDetail> PackingListDetails
        {
            get { return GetCollection<PackingListDetail>(nameof(PackingListDetails)); }
        }



        protected override void OnDeleting()
        {
            foreach (XPMemberInfo mi in ClassInfo.CollectionProperties)
            {
                if (mi.IsCollection && mi.IsAssociation)  //mi.IsAggregated &&
                {
                    if (Session.CollectReferencingObjects(this).Count > 0)
                    {
                        foreach (IXPObject obj in Session.CollectReferencingObjects(this))
                        {
                            // if (((XPBaseCollection)mi.GetValue(this)).BaseIndexOf(obj) < 0)
                            {
                                throw new InvalidOperationException("The object cannot be deleted. Other objects have references to it.");
                            }
                        }
                    }
                }
            }
            base.OnDeleting();
        }
    }
    [System.ComponentModel.DefaultProperty("FullName")]
    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Lists Archive")]
    public class THInCharge : BaseObject
    {
        public THInCharge(Session session) : base(session) { }

        [RuleRequiredField("UserName Code is required", DefaultContexts.Save, "UserName Code is required")]
        public SecuritySystemUser UserName { get; set; }

        public String FullName { get; set; }

        public String Position { get; set; }

        public Department DepartmentID { get; set; }

    }
    [System.ComponentModel.DefaultProperty("PackName")]
    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Lists Archive")]
    public class Part : BaseObject
    {
        private String partName;  //50

        public Part(Session session)
            : base(session)
        {
        }

        [Size(50)]
        public String PartName
        {
            get { return partName; }
            set { partName = value; }
        }
        [Association("ItemPack-Part", typeof(ItemPack))]
        public XPCollection ItemParts
        {
            get
            {
                return GetCollection("ItemParts"); ;
            }
        }
    }
    [System.ComponentModel.DefaultProperty("DepartmentCode")]
    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Default")]
    public class Department : BaseObject
    {
        private String departmentCode;
        private String departmentName;

        public Department(Session session)
            : base(session)
        {
        }
        [RuleRequiredField("Department Code is required", DefaultContexts.Save, "DepartmentCode is required")]
        [Size(20)]
        public String DepartmentCode
        {
            get { return departmentCode; }
            set { departmentCode = value; }
        }
        [Size(100)]
        [RuleRequiredField("Department Name is required", DefaultContexts.Save, "Department Name is required")]

        public String DepartmentName
        {
            get { return departmentName; }
            set { departmentName = value; }
        }

        public SecuritySystemUser UserApproveID { get; set; }

        public String ApproveEmail { get; set; }

        [Association("Employee-Department", typeof(Employee))]
        public XPCollection Employees
        {
            get
            {
                return GetCollection("Employees");
            }
        }
    }
    [System.ComponentModel.DefaultProperty("FullName")]
    [DefaultClassOptions, ImageName("Group")]
    [NavigationItem(GroupName = "Default")]
    public class Employee : Person
    {
        public Employee(Session session) : base(session) { }

        private SecuritySystemUser userID;
        public SecuritySystemUser UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        private Department departmentID;

        [Association("Employee-Department", typeof(Department))]
        public Department DepartmentID
        {
            get
            {
                return departmentID;
            }
            set { SetPropertyValue("DepartmentID", ref departmentID, value); }

        }

        [Size(SizeAttribute.Unlimited)]
        public String RelevantEmail { get; set; }

    }
}
