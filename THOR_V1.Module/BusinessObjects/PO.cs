using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.XtraRichEdit.API.Native;
using DevExpress.XtraRichEdit.Layout;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using THOR_V1.Module.DatabaseUpdate;

namespace THOR_V1.Module.BusinessObjects
{
    [Appearance("LockShipTo", AppearanceItemType = "ViewItem", TargetItems = "ShipTo", Criteria = "IsInputManual = false", Context = "Any", Enabled = false)]
    [System.ComponentModel.DefaultProperty("ProformaPO")]
    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Customer Order Entry")]
    public class ProformaInvoice : BaseObject
    {
        private String proformaPO; //255
        private DateTime entryDate;
        private OrderType orderTypeID;
        private POTerm pOTermID;
        private PaymentTerm paymentTermID;
        private DateTime calculationDate;
        private DateTime priceValidityDate; // EntryDate + 30        
        private InCharge inchargeID;
        private Customer customerID;
        private String comments;


        public ProformaInvoice(Session session)
            : base(session)
        {
        }

        [RuleRequiredField("ProformaInvoice Customer is required", DefaultContexts.Save, "Customer is required")]
        [Association("ProformaInvoice-Customer", typeof(Customer)), ImmediatePostData]
        public Customer CustomerID
        {
            get
            {
                return customerID;
            }
            set
            {
                if (customerID == value)
                    return;
                customerID = value;
            }
        }



        [Association("ProformaInvoice-Incharge", typeof(InCharge)), ImmediatePostData]
        public InCharge InchargeID
        {
            get
            {
                if (customerID != null)
                {
                    inchargeID = customerID.InChargeID;
                }
                return inchargeID;
            }
            set { inchargeID = value; }
        }


        [Size(255)]
        [RuleUniqueValue("", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        [RuleRequiredField("Proforma PO is required", DefaultContexts.Save, "Proforma PO is required")]

        public String ProformaPO
        {
            get { return proformaPO; }
            set { proformaPO = value; }
        }

        public DateTime EntryDate
        {
            get { return entryDate; }
            set { entryDate = value; }
        }

        [RuleRequiredField("ProformaInvoice Calculation date is required", DefaultContexts.Save, "CalculationDate is required")]
        public DateTime CalculationDate
        {
            get { return calculationDate; }
            set { calculationDate = value; }
        }

        public DateTime TentativeShipDate { get; set; }

        public override void AfterConstruction()  //Create New
        {
            base.AfterConstruction();
            PriceValidityDate = DateTime.Today.AddDays(30);
        }

        [RuleRequiredField("ProformaInvoice PaymentTerm is required", DefaultContexts.Save, "Payment Term is required")]
        [Association("ProformaInvoice-PaymentTerm", typeof(PaymentTerm))]

        public PaymentTerm PaymentTermID
        {
            get { return paymentTermID; }
            set { paymentTermID = value; }
        }
        //[RuleRequiredField("ProformaInvoice Order Type is required", DefaultContexts.Save, "Order Type is required")]
        [Association("ProformaInvoice-OrderType", typeof(OrderType))]
        public OrderType OrderTypeID
        {
            get { return orderTypeID; }
            set { orderTypeID = value; }
        }
        [RuleRequiredField("ProformaInvoice PO Term Type is required", DefaultContexts.Save, "PO Term is required")]
        [Association("ProformaInvoice-POTerm", typeof(POTerm))]
        public POTerm POTermID
        {
            get
            {

                return pOTermID;

            }
            set
            {
                pOTermID = value;
            }
        }

        //********** phuongnv******************************
        // Edit date: 01/10/2017 
        // Description:  

        public Double Discount
        {
            get;
            set;
            /*get {
                if (CustomerID != null)
                {
                    discount = CustomerID.Discount;
                }
                return discount; 
            }*/

        }

        /*
         public Double Discount
        {
            get
            {
                
                Double discount = 0;
                CriteriaOperator filterpo = new BinaryOperator("CustomerID", this.CustomerID.Oid);
               // filterpo = filterpo & new BinaryOperator("OrderTypeID", this.OrderTypeID.Oid);
                filterpo = filterpo & new BinaryOperator("POTermID", this.POTermID.Oid);
                XPCollection<CustomerDiscount> css = new XPCollection<CustomerDiscount>(Session, filterpo);

                foreach (CustomerDiscount cd in css)
                {
                    if (cd != null) //cd.ValidFrom <= POID.CalculationDate)
                    {
                        discount = cd.POTermID.UnitPriceFactor;
                        break;
                    }
                    else
                        discount = 1; /// error Discount not found
                }
                return discount;
            }
        }
        */
        [Size(255)]
        public String Comments
        {
            get { return comments; }
            set { comments = value; }
        }

        private String billTo;

        [Size(500)]
        public String BillTo
        {
            get
            {
                //if (!IsSaving && !IsLoading && !IsDeleted && !IsInputManual)
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



        public bool IsInputManual { get; set; }


        private String shipTo;
        [Size(500)]
        public String ShipTo
        {
            get
            {
                if (!IsSaving && !IsLoading && !IsDeleted && !IsInputManual)
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

        public DateTime ETDRequest { get; set; }

        public DateTime PriceValidityDate
        {
            get { return priceValidityDate; }
            set { priceValidityDate = value; }
        }
        [Association("ProformaInvoiceDetail-ProformaInvoice", typeof(ProformaInvoiceDetail))]
        public XPCollection ProformaInvoiceDetails
        {
            get
            {
                return GetCollection("ProformaInvoiceDetails");
            }
        }
        public bool POClosed { get; set; }


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
    [RuleCriteria("Proforma PO Qty is greater than 0", DefaultContexts.Save, "Qty > 0", "Proforma PO Qty is greater than 0", SkipNullOrEmptyValues = false)]
    //[RuleCriteria("ProformaInvoiceDetail Qty must greater than MOQQty", DefaultContexts.Save, "Qty >= MOQQty", "ProformaInvoiceDetail Qty must greater than MOQQty", SkipNullOrEmptyValues = false)]
    //[Appearance("OverMOQ", AppearanceItemType = "ViewItem", TargetItems = "*", BackColor = "LightCoral", Criteria = "MOQQty > Qty", FontColor = "WindowText", Context = "ListView")]
    [System.ComponentModel.DefaultProperty("UniqueName")]
    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Customer Order Entry")]
    public class ProformaInvoiceDetail : BaseObject
    {
        private ProformaInvoice proformaInvoiceID;
        private Item itemID;
        private Component componentID;


        private int qty;
        private String specialRequest;
        private Double unitPrice;


        public ProformaInvoiceDetail(Session session)
            : base(session)
        {
        }

        [Association("ProformaInvoiceDetail-ProformaInvoice", typeof(ProformaInvoice))] //ok
        public ProformaInvoice ProformaInvoiceID
        {
            get { return proformaInvoiceID; }
            set { proformaInvoiceID = value; }
        }
        //[RuleRequiredField("ProformaInvoice Item is required", DefaultContexts.Save, "PO Item is required")]
        [Association("ProformaInvoiceDetail-Item", typeof(Item))] //ok
        public Item ItemID
        {
            get { return itemID; }
            set { itemID = value; }
        }

        private DesignerBy designerByID;
        [Persistent]
        public DesignerBy DesignerByID
        {
            get => designerByID;

        }
        public Component ComponentID
        {
            get { return componentID; }
            set { componentID = value; }
        }
        public int Qty
        {
            get { return qty; }
            set { qty = value; }
        }

        //private string discount = "";
        //public string DiscountWithDesign
        //{
        //    get
        //    {
        //        return discount;
        //    }
        //}

        private Double specialDiscount;

        [Custom("EditMask", "##,###,###.00")]
        public Double SpecialDiscount
        {
            get { return specialDiscount; }
            set { specialDiscount = value; }
        }

        //private Double underMOQCharge;
        //[Custom("EditMask", "##,###,###.00")]
        //public Double UnderMOQCharge
        //{
        //    get { return underMOQCharge; }
        //    set { underMOQCharge = value; }
        //}

        private Double extraCharge;
        [Custom("EditMask", "###,###,###.00")]
        public Double ExtraCharge
        {
            get { return Math.Round(extraCharge, 2); }
            set { extraCharge = value; }
        }

        [Custom("EditMask", "###,###,###.00")]
        [Persistent]
        public Double UnitPrice
        {
            get
            {
                return Math.Round(unitPrice, 2);
            }
            set { unitPrice = value; }
        }

        private Double invoicePrice;
        [Custom("EditMask", "###,###,###.00")]
        public Double InvoicePrice
        {
            get
            {
                if (!IsLoading && !IsDeleted && !IsSaving)
                {
                    if (SpecialDiscount > 0)
                        invoicePrice = (UnitPrice + ExtraCharge) * (1 - SpecialDiscount / 100);
                    else
                        invoicePrice = (UnitPrice + ExtraCharge);

                }
                return Math.Round(invoicePrice, 2);
            }
            set { invoicePrice = value; }
        }

        [Size(255)]
        public String SpecialRequest
        {
            get { return specialRequest; }
            set { specialRequest = value; }
        }

        public DateTime ETDRequest { get; set; }

        [Persistent]
        [RuleUniqueValue("", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        public String UniqueName
        {
            get
            {
                if (ProformaInvoiceID == null)
                    return ItemID == null ? "" : ItemID.ToString();
                else
                    return ProformaInvoiceID.ToString() + '-' + (ItemID == null ? "" : ItemID.ToString());
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

        protected override void OnSaving()
        {
            //if (specialDiscount >0)
            //    unitPrice = unitPrice * (1 - specialDiscount / 100);

            base.OnSaving();
        }


    }

    [System.ComponentModel.DefaultProperty("CustPO")]
    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Customer Order Entry")]
    public class PO : BaseObject
    {
        private Customer customerID;
        private String custPO; //255
        private DateTime custPOReleaseDate;

        private DateTime confirmDate;
        //private OrderType orderTypeID;
        private FreightType freightTypeID;
        private POTerm pOTermID;
        private OrderType orderTypeID;
        private double discount;
        private bool isDummyPO;
        private String comments;
        private String remark;
        private String importedBy;//priceComments;
        private String forCustomer;
        private DateTime calculationDate;
        private bool isCreatedSO;
        private String specialRequest;
        //private bool isPOClosed;
        private bool pOClosed;


        public PO(Session session)
            : base(session)
        {
        }

        [Persistent]
        [RuleRequiredField("PO Customer is required", DefaultContexts.Save, "PO Customer is required")]
        [Association("PO-Customer", typeof(Customer)), ImmediatePostData]
        public Customer CustomerID
        {
            get { return customerID; }
            set
            {
                customerID = value;
            }
        }
        [Size(255)]
        [RuleUniqueValue("", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        [RuleRequiredField("CustPO is required", DefaultContexts.Save, "CustPO is required")]



        public String CustPO
        {
            get { return custPO; }
            set { custPO = value; }
        }

        public DateTime CustPOReleaseDate
        {
            get { return custPOReleaseDate; }
            set { custPOReleaseDate = value; }
        }
        [RuleRequiredField("PO Calculation date is required", DefaultContexts.Save, "CalculationDate is required")]
        public DateTime CalculationDate
        {
            get { return calculationDate; }
            set { calculationDate = value; }
        }


        public DateTime ConfirmDate
        {
            get { return confirmDate; }
            set { confirmDate = value; }
        }
        [Persistent]
        [RuleRequiredField("PO Term ID is required", DefaultContexts.Save, "PO Term ID is required")]
        [Association("PO-POTerm", typeof(POTerm)), ImmediatePostData]
        public POTerm POTermID
        {
            get
            {


                return pOTermID;

            }

            set { pOTermID = value; }
        }

        public String TentativeShipDate { get; set; }

        [Persistent]
        //[RuleRequiredField("PO Order Type is required", DefaultContexts.Save, "PO Order Type is required")]
        [Association("PO-FreightType", typeof(FreightType)), ImmediatePostData]
        public FreightType FreightTypeID
        {
            get { return freightTypeID; }
            set { freightTypeID = value; }
        }

        [Persistent]
        // [RuleRequiredField("PO Order Type is required", DefaultContexts.Save, "PO Order Type is required")]
        [Association("PO-OrderType", typeof(OrderType)), ImmediatePostData]
        public OrderType OrderTypeID
        {
            get { return orderTypeID; }
            set { orderTypeID = value; }
        }

        public Double Discount
        {
            get
            {

                return discount;
            }
            set
            {
                discount = value;
            }
        }
        //public Double DefaultDiscount
        //{
        //    get
        //    {
        //        if (CustomerID != null)
        //        {
        //            discount = CustomerID.Discount;
        //        }
        //        else
        //        {
        //            discount = 0;
        //        }

        //        return discount;
        //    }
        //}

        public bool IsDummyPO
        {
            get { return isDummyPO; }
            set { isDummyPO = value; }
        }
        [Size(255)]
        public String SpecialRequest
        {
            get
            {
                return specialRequest;
            }
            set
            {
                if (specialRequest == value)
                    return;
                specialRequest = value;
            }
        }

        [Size(255)]
        public String ImportedBy
        {
            get
            {
                return importedBy;
            }
            set
            {
                if (importedBy == value)
                    return;
                importedBy = value;
            }
        }
        [Size(255)]
        public String Comments
        {
            get { return comments; }
            set { comments = value; }
        }
        public String Remark
        {
            get { return remark; }
            set { remark = value; }
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

        public bool IsCreatedSO
        {
            get { return isCreatedSO; }
            set { isCreatedSO = value; }
        }
        [Size(255)]
        public String ForCustomer
        {
            get { return forCustomer; }
            set { forCustomer = value; }
        }

        [Association("PODetail-PO", typeof(PODetail))]
        public XPCollection PODetails
        {
            get
            {
                return GetCollection("PODetails");
            }
        }

        public bool POClosed
        {
            get { return pOClosed; }
            set { pOClosed = value; }
        }

        [Association("CancelPO-PO", typeof(CancelPO))]
        public XPCollection CancelPOs
        {
            get { return GetCollection("CancelPOs"); }
        }
    }

    [System.ComponentModel.DefaultProperty("CustShipNo")]
    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Customer Order Entry")]
    public class Shipment : BaseObject
    {
        private Customer customerID;
        private String custShipNo;
        private String containerNo;
        private DateTime shipDate;
        private String comments;
        private bool isConfirm;
        private CustomerAddress shipToID;
        private CustomerAddress billToID;
        private InCharge inchargeID;
        private DateTime deliveryDate;
        private ShipmentTerm shipmentTermID;
        private Double debitAmt;

        private Double creditAmt;
        private Double freightSurcharge;
        private String reason; //200
        private ContainerSize containerID;

        public Shipment(Session session)
            : base(session)
        {
        }
        [Association("Shipment-Customer", typeof(Customer))] //ok
        public Customer CustomerID
        {
            get { return customerID; }
            set { customerID = value; }
        }

        [RuleUniqueValue("", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        [Size(50)]
        public String CustShipNo
        {
            get { return custShipNo; }
            set { custShipNo = value; }
        }
        [Size(50)]
        public String ContainerNo
        {
            get { return containerNo; }
            set { containerNo = value; }
        }
        [RuleRequiredField("Shipment Container Size is required", DefaultContexts.Save, "Container Size is required")]
        [Association("Shipment-ContainerSize", typeof(ContainerSize))]
        public ContainerSize ContainerID
        {
            get
            {
                return containerID;
            }
            set
            {
                if (containerID == value)
                    return;
                containerID = value;
            }
        }
        public DateTime ShipDate
        {
            get { return shipDate; }
            set { shipDate = value; }
        }
        [Size(255)]
        public String Comments
        {
            get { return comments; }
            set { comments = value; }
        }

        public bool IsConfirm
        {
            get
            {
                int sdq = 0;
                int sdpq = 0;
                if (this.Oid != null && Session.Connection != null)
                {
                    SortProperty sortpo = null;
                    CriteriaOperator filterpo = new BinaryOperator("ShipmentID", this.Oid);
                    XPCollection<ShipmentDetail> css = new XPCollection<ShipmentDetail>(Session, filterpo, sortpo);

                    isConfirm = true;
                    foreach (ShipmentDetail cd in css)
                    {
                        sdq = cd.QtyShip;
                        sdpq = 0;
                        CriteriaOperator filterpod = new BinaryOperator("ShipmentDetailID", cd.Oid);
                        XPCollection<ShipmentDetailPO> csp = new XPCollection<ShipmentDetailPO>(Session, filterpod, sortpo);
                        foreach (ShipmentDetailPO sdp in csp)
                        {
                            sdpq = sdpq + sdp.QtyShipPO;
                        }
                        if (sdq != sdpq && cd.IsReplace == false)
                            isConfirm = false;
                    }
                    return isConfirm;
                }
                return false;

            }

        }
        public Double FreightSurcharge
        {
            get
            {
                return freightSurcharge;
            }
            set
            {
                if (freightSurcharge == value)
                    return;
                freightSurcharge = value;
            }
        }
        // Do Filter by Customer
        [Association("ShipmentShipTo-CustomerAddress", typeof(CustomerAddress))]
        public CustomerAddress ShipToID
        {
            get { return shipToID; }
            set { shipToID = value; }
        }
        // Do Filter by Customer
        [Association("ShipmentBillTo-CustomerAddress", typeof(CustomerAddress))]
        public CustomerAddress BillToID
        {
            get { return billToID; }
            set { billToID = value; }
        }
        [Association("Shipment-InCharge", typeof(InCharge))]
        public InCharge InchargeID
        {
            get { return inchargeID; }
            set { inchargeID = value; }
        }

        [RuleRequiredField("Shipment DeliveryDate is required", DefaultContexts.Save, "Delivery Date is required")]
        public DateTime DeliveryDate
        {
            get { return deliveryDate; }
            set { deliveryDate = value; }
        }

        public Double CreditAmt
        {
            get { return creditAmt; }
            set { creditAmt = value; }
        }
        public Double DebitAmt
        {
            get { return debitAmt; }
            set { debitAmt = value; }
        }
        [Size(200)]
        public String Reason
        {
            get { return reason; }
            set { reason = value; }
        }

        public String PathFile { get; set; }

        public bool CreatedInvoice { get; set; }

        //private FileData _file;
        //[Aggregated, ExpandObjectMembers(ExpandObjectMembers.Never)]
        //public FileData LoadContFile
        //{
        //    get => _file;
        //    set => SetPropertyValue(nameof(LoadContFile), ref _file, value);
        //}

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
        [Association("Shipment-ShipmentTerm", typeof(ShipmentTerm))]
        public ShipmentTerm ShipmentTermID
        {
            get { return shipmentTermID; }
            set { shipmentTermID = value; }
        }
        [Association("ShipmentDetail-Shipment", typeof(ShipmentDetail))]
        public XPCollection ShipmentDetails
        {
            get
            {
                return GetCollection("ShipmentDetails");
            }
        }

    }

    [RuleCriteria("Shipment Qty is greater than 0", DefaultContexts.Save, "QtyShip > 0", "Shipment Qty is greater than 0", SkipNullOrEmptyValues = false)]

    [System.ComponentModel.DefaultProperty("ShipmentID")]
    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Customer Order Entry")]
    public class ShipmentDetail : BaseObject
    {
        private Shipment shipmentID;
        private Item itemID;
        private int qtyShip;
        private bool isConfirm;
        private String otherGoods;
        private Double goodsPrice;
        private bool isReplace;

        private String comments;

        public ShipmentDetail(Session session)
            : base(session)
        {
        }
        [Association("ShipmentDetail-Shipment", typeof(Shipment))] //ok

        public Shipment ShipmentID
        {
            get { return shipmentID; }
            set { shipmentID = value; }
        }
        [Association("ShipmentDetail-Item", typeof(Item))] //ok
        public Item ItemID
        {
            get { return itemID; }
            set { itemID = value; }
        }

        public int QtyShip
        {
            get { return qtyShip; }
            set { qtyShip = value; }
        }
        [Size(100)]
        public String OtherGoods
        {
            get { return otherGoods; }
            set { otherGoods = value; }
        }


        public Double GoodsPrice
        {
            get { return goodsPrice; }
            set { goodsPrice = value; }
        }
        public bool IsReplace
        {
            get { return isReplace; }
            set { isReplace = value; }
        }
        [Size(100)]
        public String Comments
        {
            get { return comments; }
            set { comments = value; }
        }
        public bool IsConfirm
        {
            get
            {
                int sdpq = 0;
                if (this.Oid != null && Session.Connection != null)
                {

                    isConfirm = true;
                    if (IsReplace == false || OtherGoods == "")
                    {
                        sdpq = 0;
                        SortProperty sortpo = null;
                        CriteriaOperator filterpod = new BinaryOperator("ShipmentDetailID", this.Oid);
                        XPCollection<ShipmentDetailPO> csp = new XPCollection<ShipmentDetailPO>(Session, filterpod, sortpo);
                        foreach (ShipmentDetailPO sdp in csp)
                        {
                            sdpq = sdpq + sdp.QtyShipPO;
                        }
                        if (QtyShip != sdpq)
                            isConfirm = false;
                    }
                    return isConfirm;
                }
                return false;
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
                            // // if (((XPBaseCollection)mi.GetValue(this)).BaseIndexOf(obj) < 0)
                            {
                                throw new InvalidOperationException("The object cannot be deleted. Other objects have references to it.");
                            }
                        }
                    }
                }
            }
            base.OnDeleting();
        }
        [Association("ShipmentDetailPO-ShipmentDetail", typeof(ShipmentDetailPO))] //ok
        public XPCollection ShipmentDetailPOs
        {
            get
            {
                return GetCollection("ShipmentDetailPOs");
            }
        }
        [Persistent]
        [RuleUniqueValue("", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        public String UniqueName
        {
            get
            {
                if (ShipmentID == null)
                    return (ItemID == null ? "" : ItemID.Oid.ToString()) + (OtherGoods == null ? "" : OtherGoods.ToString());
                else
                    return ShipmentID.ToString() + '-' + (ItemID == null ? "" : ItemID.ToString()) + (OtherGoods == null ? "" : OtherGoods.ToString());
            }

        }
    }

    [System.ComponentModel.DefaultProperty("CancelPONum")]
    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Customer Order Entry")]
    public class CancelPO : BaseObject
    {
        private int cancelPONum;
        private PO poID;
        private DateTime createDate;
        private DateTime cancelDate;
        private String reason;
        private String comments;
        private bool isConfirm;

        public CancelPO(Session session)
            : base(session)
        {
        }
        [Persistent]
        [RuleUniqueValue("", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        public int CancelPONum
        {
            get { return cancelPONum; }
            set { cancelPONum = value; }
        }
        [Association("CancelPO-PO", typeof(PO))] //ok
        public PO POID
        {
            get { return poID; }
            set { poID = value; }
        }
        public DateTime CreateDate
        {
            get { return createDate; }
            set { createDate = value; }
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            CreateDate = DateTime.Today;
            CancelDate = DateTime.Today;
        }

        public DateTime CancelDate
        {
            get { return cancelDate; }
            set { cancelDate = value; }
        }

        [Size(255)]
        public String Reason
        {
            get { return reason; }
            set { reason = value; }
        }
        [Size(255)]
        public String Comments
        {
            get { return comments; }
            set { comments = value; }
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (CancelPONum == 0 && Session.Connection != null)
            {
                SqlConnection connection = (SqlConnection)Session.Connection;
                string myQuery = "NextCancelPONumGetStp";
                SqlCommand myCommand = new SqlCommand();
                myCommand.CommandText = myQuery;
                myCommand.Connection = connection;
                myCommand.CommandType = CommandType.StoredProcedure;
                SqlParameter Param = myCommand.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
                Param.Direction = ParameterDirection.ReturnValue;
                myCommand.ExecuteNonQuery();
                CancelPONum = (int)myCommand.Parameters["RETURN_VALUE"].Value;
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
        public bool IsConfirm
        {
            get { return isConfirm; }
            set { isConfirm = value; }

        }
        [Association("CancelPO-CancelPODetail", typeof(CancelPODetail))]
        public XPCollection CancelPODetails
        {
            get
            {
                return GetCollection("CancelPODetails");
            }
        }


    }

    [RuleCriteria("Cancel Qty is greater than 0", DefaultContexts.Save, "QtyCancel > 0", "Qty Cancel is greater than 0", SkipNullOrEmptyValues = false)]
    // [RuleCriteria("CancelPODetailPO Qty Criteria", DefaultContexts.Save, "CancelPODetailID.CancelPODetailPOs.Sum(QtyCancelPO) <= CancelPODetailID.QtyCancel ", "Total QtyCancelPO is equal QtyCancel", SkipNullOrEmptyValues = false)]
    //[RuleCriteria("Cancel Qty PO is greater than 0", DefaultContexts.Save, "QtyCancelPO > 0", "Qty Cancel PO is greater than 0", SkipNullOrEmptyValues = false)]
    [RuleCriteria("PODetail Outstanding Allocate cancel is greater or equal 0", DefaultContexts.Save, "PODetailQty >= 0", "PODetail Outstanding Allocate is greater or equal 0", SkipNullOrEmptyValues = false)]
    [RuleCriteria("PODetail Must be greater QtyCancel", DefaultContexts.Save, "PODetailQty >= QtyCancel", "PODetail Must be greater QtyCancel", SkipNullOrEmptyValues = false)]

    [System.ComponentModel.DefaultProperty("CancelPOID")]
    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Customer Order Entry")]
    public class CancelPODetail : BaseObject
    {
        private PODetail poDetailID;
        private CancelPO cancelPOID;
        //    private Item itemID;
        private int qtyCancel;

        public CancelPODetail(Session session)
            : base(session)
        {
        }

        [Association("CancelPO-CancelPODetail", typeof(CancelPO))] //ok
        public CancelPO CancelPOID
        {
            get { return cancelPOID; }
            set
            {
                cancelPOID = value;
                if (!IsLoading)
                {
                    OnChanged("CancelPOID");
                    UpdateCancelPOList();
                }
            }
        }
        //[Association("CancelPODetail-Item",typeof(Item))] //ok
        //public Item ItemID
        //{
        //  get { return itemID; }
        //  set { itemID = value; }
        //}

        private XPCollection<PODetail> cancelpo = null;
        public XPCollection<PODetail> CancelPODataSource
        {
            get
            {
                if (cancelpo == null)
                {
                    cancelpo = new XPCollection<PODetail>(Session);
                }
                UpdateCancelPOList();
                return cancelpo;
            }
        }
        private void UpdateCancelPOList()
        {

            if (cancelpo != null)
                cancelpo.Criteria = CancelPOID != null ? CriteriaOperator.Parse("POID = ?", CancelPOID.POID) : null;
        }


        [Persistent]
        [DataSourceProperty("CancelPODataSource")]
        [RuleRequiredField("Cancel PO is required", DefaultContexts.Save, "PODetail is required")]
        [Association("CancelPODetail-PODetail", typeof(PODetail))]
        public PODetail PODetailID
        {
            get { return poDetailID; }
            set { poDetailID = value; }
        }

        public int QtyCancel
        {
            get
            {
                if (!IsLoading && !IsSaving && !IsDeleted && PODetailID != null && qtyCancel == 0)
                {
                    qtyCancel = PODetailID.QtyOutstanding;
                }
                return qtyCancel;
            }
            set { qtyCancel = value; }
        }
        public int PODetailQty
        {
            get
            {
                int spq = 0;
                int cq = 0;
                int pdq = 0;
                if (!IsLoading && !IsSaving)
                {
                    if (PODetailID != null && Session.Connection != null)
                    {
                        pdq = PODetailID.QtyOutstanding;
                        /*SortProperty sortpo = null;
                        CriteriaOperator filterpo = new BinaryOperator("PODetailID", this.PODetailID.Oid);
                        XPCollection<ShipmentDetailPO> css = new XPCollection<ShipmentDetailPO>(Session, filterpo, sortpo);

                        foreach (ShipmentDetailPO cd in css)
                        {
                            spq = spq + cd.QtyShipPO;
                        }

                        XPCollection<CancelPODetail> cs = new XPCollection<CancelPODetail>(Session, filterpo, sortpo);
                        foreach (CancelPODetail c in cs)
                        {
                            if (c.Oid != this.Oid)
                                cq = cq + c.QtyCancel;
                        }*/
                    }
                }
                return pdq;
            }
        }
        [Persistent]
        [RuleUniqueValue("", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        public String UniqueName
        {
            get
            {
                if (CancelPOID == null)
                    return PODetailID == null ? "" : PODetailID.ToString();
                else
                    return CancelPOID.ToString() + '-' + (PODetailID == null ? "" : PODetailID.ToString());
            }

        }
    }

    [RuleCriteria("ShipmentDetailPO Qty Criteria", DefaultContexts.Save, "ShipmentDetailID.ShipmentDetailPOs.Sum(QtyShipPO) <= ShipmentDetailID.QtyShip ", "Total QtyShipPO is equal QtyShip", SkipNullOrEmptyValues = false)]
    [RuleCriteria("Shipment Qty PO is greater than 0", DefaultContexts.Save, "QtyShipPO > 0", "Shipment Qty PO is greater than 0", SkipNullOrEmptyValues = false)]
    [RuleCriteria("PODetail Outstanding Allocate shipment is greater or equal 0", DefaultContexts.Save, "PODetailQty >= 0", "PODetail Outstanding Allocate is greater or equal 0", SkipNullOrEmptyValues = false)]
    [System.ComponentModel.DefaultProperty("ShipmentDetailID")]
    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Customer Order Entry")]
    public class ShipmentDetailPO : BaseObject
    {
        private ShipmentDetail shipmentDetailID;
        private PODetail poDetailID;
        private int qtyShipPO;

        private String comments;

        public ShipmentDetailPO(Session session)
            : base(session)
        {
        }
        [RuleRequiredField("ShipmentDetail is required", DefaultContexts.Save, "ShipmentDetail is required")]
        [Association("ShipmentDetailPO-ShipmentDetail", typeof(ShipmentDetail))] //ok
        public ShipmentDetail ShipmentDetailID
        {
            get { return shipmentDetailID; }
            set
            {
                shipmentDetailID = value;
                if (!IsLoading)
                {
                    OnChanged("ShipmentDetailID");
                    UpdateShipmentPODetailList();
                }
            }
        }

        private XPCollection<PODetail> shipmentpo = null;
        public XPCollection<PODetail> ShipmentDataSource
        {
            get
            {
                if (shipmentpo == null)
                {
                    shipmentpo = new XPCollection<PODetail>(Session);
                }
                UpdateShipmentPODetailList();
                return shipmentpo;
            }
        }
        private void UpdateShipmentPODetailList()
        {

            if (shipmentpo != null)
                shipmentpo.Criteria = ShipmentDetailID != null ? CriteriaOperator.Parse("ItemID = ?", ShipmentDetailID.ItemID) : null;
        }

        [Persistent]
        [DataSourceProperty("ShipmentDataSource")]
        [RuleRequiredField("Shipment PODetail is required", DefaultContexts.Save, "PODetail is required")]
        [Association("ShipmentDetailPO-PODetail", typeof(PODetail))] //ok
        public PODetail PODetailID
        {
            get { return poDetailID; }
            set { poDetailID = value; }
        }

        [Persistent]
        public int QtyShipPO
        {
            get { return qtyShipPO; }
            set { qtyShipPO = value; }
        }

        public int PODetailQty
        {
            get
            {
                int spq = 0;
                int cq = 0;
                int pdq = 0;
                if (PODetailID != null && Session.Connection != null)
                {
                    pdq = PODetailID.Qty;

                    SortProperty sortpo = null;
                    CriteriaOperator filterpo = new BinaryOperator("PODetailID", this.PODetailID.Oid);
                    XPCollection<ShipmentDetailPO> css = new XPCollection<ShipmentDetailPO>(Session, filterpo, sortpo);

                    foreach (ShipmentDetailPO cd in css)
                    {
                        if (cd.Oid != this.Oid)
                            spq = spq + cd.QtyShipPO;
                    }

                    XPCollection<CancelPODetail> cs = new XPCollection<CancelPODetail>(Session, filterpo, sortpo);
                    foreach (CancelPODetail c in cs)
                    {
                        cq = cq + c.QtyCancel;
                    }
                }

                return pdq - spq - cq - this.QtyShipPO;
            }
        }
        [Size(100)]
        public String Comments
        {
            get { return comments; }
            set { comments = value; }
        }
        [Association("ShipmentDetailPOPacking-ShipmentDetailPO", typeof(ShipmentDetailPOPacking))]
        public XPCollection ShipmentDetailPOPackings
        {
            get
            {
                return GetCollection("ShipmentDetailPOPackings");
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

    [RuleCriteria("POOutstanding Qty Criteria", DefaultContexts.Save, "QtyOutstanding >=0 ", "POOutstanding Qty is greater than or equal 0", SkipNullOrEmptyValues = false)]
    [RuleCriteria("PO Qty is greater than 0", DefaultContexts.Save, "Qty > 0", "PO Qty is greater than 0", SkipNullOrEmptyValues = false)]
    [System.ComponentModel.DefaultProperty("UniqueName")]
    [DefaultClassOptions, ImageName("BO_Product")]
    [DefaultListViewOptions(true, NewItemRowPosition.None)]
    [NavigationItem(GroupName = "Customer Order Entry")]
    public class PODetail : BaseObject
    {
        private PO poID;
        private Item itemID;
        private Component componentID;


        private int qty;
        private String specialRequest;
        private String commentsRpt44;
        private Double unitPrice;
        private Double invoicePrice;
        private Double importPrice;
        //private Double refPrice;


        //private Double discount;
        private DateTime requestDate;
        private DateTime confirmDate;
        private String fullSizeVersion;


        public PODetail(Session session) : base(session) { }

        [Association("PODetail-PO", typeof(PO))] //ok
        public PO POID
        {
            get { return poID; }
            set { poID = value; }
        }
        // [RuleRequiredField("PO Item is required", DefaultContexts.Save, "PO Item is required")]
        [Association("PODetail-Item", typeof(Item))] //ok
        public Item ItemID
        {
            get { return itemID; }
            set { itemID = value; }
        }

        public Component ComponentID
        {
            get { return componentID; }
            set { componentID = value; }
        }

        [Size(255)]
        public DateTime RequestDate
        {
            get
            {
                return requestDate;
            }
            set
            {
                if (requestDate == value)
                    return;
                requestDate = value;
            }
        }
        public DateTime ConfirmDate
        {
            get
            {

                return confirmDate;
            }
            set
            {
                confirmDate = value;
            }
        }
        public int Qty
        {
            get { return qty; }
            set { qty = value; }
        }

        private string discountWithDesign = "";
        [Persistent]
        public string DiscountWithDesign
        {
            get
            {
                /*if (itemID != null && poID != null)
                {
                    if (itemID.DesignerByID != null)
                    {
                        if (poID.CustomerID.ShortName.Equals("SAR") && itemID.DesignerByID.DesignerInfo.Equals("BERGELIN"))
                            discountWithDesign = "5%";
                    }
                }*/
                return discountWithDesign;
            }
        }

        [Custom("EditMask", "##,###,###.00")]
        public string SpecialDiscount { get; set; }

        private Double underMOQCharge;
        [Custom("EditMask", "##,###,###.00")]
        public Double UnderMOQCharge
        {
            get { return underMOQCharge; }
            set { underMOQCharge = value; }
        }

        private Double extraCharge;
        public Double ExtraCharge
        {
            get { return extraCharge; }
            set { extraCharge = value; }
        }

        [Custom("EditMask", "###,###,###.00")]
        public Double UnitPrice
        {
            get { return unitPrice; }
            set { unitPrice = value; }
        }
        [Custom("EditMask", "###,###,###.00")]
        public Double InvoicePrice
        {
            get { return invoicePrice; }
            set { invoicePrice = value; }
        }
        [Custom("EditMask", "###,###,###.00")]
        public Double ImportPrice
        {
            get
            {
                return importPrice;
            }
            set
            {
                if (importPrice == value)
                    return;
                importPrice = value;
            }
        }

        private int qtycancelpo;

        
        public int QtyCancel
        {
            get =>  qtycancelpo;
            set => qtycancelpo = value;

        }

        private int qtyship;

        
        public int QtyShip
        {
            get => qtyship;
            set => qtyship = value;
            /*{
                int qtyshippo = 0;
                if (this.Oid != null)
                    if (Session.Connection != null)
                    {
                        SortProperty sortpo = null;// new SortProperty("ValidFrom", SortingDirection.Descending);
                        CriteriaOperator filterpo = new BinaryOperator("PODetailID", this.Oid);
                        XPCollection<ShipmentDetailPO> cpdp = new XPCollection<ShipmentDetailPO>(Session, filterpo, sortpo);
                        foreach (ShipmentDetailPO cd in cpdp)
                        {
                            qtyshippo = qtyshippo + cd.QtyShipPO;
                        }
                    }
                return qtyshippo;
            }*/
        }

       

        private int qtyOutstanding;
        [Persistent]
        public int QtyOutstanding
        {
            get
            {
                
                int qty = 0;
                if (Qty == 0)
                    qty = 0;
                else
                    qty = Qty;

                if (Session.IsObjectsSaving == false)
                {                    
                    qtyOutstanding = qty - QtyShip - QtyCancel;
                }
                return qtyOutstanding;
            }
        }

        

        private int tagship = 0;
        
        public int TagShip
        {

            get => tagship;
            set => tagship = value;

        }
        [Persistent]
        //[RuleUniqueValue("", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        public String UniqueName
        {
            get
            {
                if (POID == null)
                    return ItemID == null ? "" : ItemID.ToString();
                else
                    return POID.ToString() + '-' + (ItemID == null ? "" : ItemID.ToString());
            }

        }
        [Size(255)]
        public String SpecialRequest
        {
            get { return specialRequest; }
            set { specialRequest = value; }
        }
        [Size(255)]
        public String CommentsRpt44
        {
            get { return commentsRpt44; }
            set { commentsRpt44 = value; }
        }

        public String FullSizeVersion
        {
            get { return fullSizeVersion; }
            set { fullSizeVersion = value; }
        }
        //[Association("ShipmentDetailPO-PODetail", typeof(ShipmentDetailPO))]
        //public XPCollection ShipmentDetailPOs
        //{
        //    get
        //    {
        //        return GetCollection("ShipmentDetailPOs");
        //    }
        //}
        [Association("CancelPODetail-PODetail", typeof(CancelPODetail))]
        public XPCollection CancelPODetails
        {
            get
            {
                return GetCollection("CancelPODetails");
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

        [Association("Tag-PODetail", typeof(Tag))]
        public XPCollection Tags
        {
            get { return GetCollection("Tags"); }
        }

        [Association("ShipmentDetailPO-PODetail", typeof(ShipmentDetailPO))]
        public XPCollection ShipmentDetailPOs
        {
            get
            {
                return GetCollection("ShipmentDetailPOs");
            }
        }

        protected override void OnSaved()
        {
            base.OnSaved();
            /*if (ItemID != null && Qty > 0)
            {
                if (ConstantObject.oldPOItemCode != ItemID.ItemCode || ConstantObject.oldPOQty != Qty && POID != null)
                {
                    BusinessExecute exe = new BusinessExecute();
                    exe.UpdateMasterPlanChangedPO(ConstantObject.oldPOItemCode, ItemID.ItemCode, ConstantObject.oldPOQty, qtyOutstanding, POID.Oid.ToString());
                }
            }*/
        }
    }

    [System.ComponentModel.DefaultProperty("InvoiceNum")]
    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Customer Order Entry")]
    public class Invoice : BaseObject
    {
        public Invoice(Session session) : base(session) { }

        [RuleUniqueValue("", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        [RuleRequiredField("InvoiceNum is required", DefaultContexts.Save, "InvoiceNum is required")]
        public String InvoiceNum { get; set; }

        private Customer customerID;
        [Association("Invoice-Customer", typeof(Customer)), ImmediatePostData]
        public Customer CustomerID
        {
            get => customerID;
            set
            {
                SetPropertyValue(nameof(CustomerID), ref customerID, value);
                //if (SetPropertyValue(nameof(CustomerID), ref customerID, value))
                //{   
                //    OnChanged(nameof(ShipmentSources));
                //}
            }
        }

        //private XPCollection<Shipment> shipmentid = null;
        //public XPCollection<Shipment> ShipmentSources
        //{
        //    get
        //    {
        //        if (shipmentid == null)
        //        {
        //            if(CustomerID!=null)
        //                shipmentid = new XPCollection<Shipment>(Session);
        //        }
        //        UpdateShipmentList();
        //        return shipmentid;
        //    }
        //}
        //private void UpdateShipmentList()
        //{

        //    if (shipmentid != null)
        //        shipmentid.Criteria = CustomerID != null ? CriteriaOperator.Parse("CustomerID = ?", CustomerID) : null;
        //}


        [Association("InvoiceShipment-Invoice", typeof(InvoiceShipment))]
        public XPCollection InvoiceShipments
        {
            get
            {
                return GetCollection("InvoiceShipments");
            }
        }

        [Association("InvoiceDetail-Invoice", typeof(InvoiceDetail))]
        public XPCollection InvoiceDetails
        {
            get
            {
                return GetCollection("InvoiceDetails");
            }
        }
    }

    [System.ComponentModel.DefaultProperty("InvoiceNum")]
    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Customer Order Entry")]
    public class InvoiceShipment : BaseObject
    {
        public InvoiceShipment(Session session) : base(session) { }

        [Association("InvoiceShipment-Invoice", typeof(Invoice))]
        public Invoice InvoiceID { get; set; }


        [DataSourceProperty(nameof(AvailableShipments))]
        public Shipment ShipmentID { get; set; }

        [Browsable(false)]
        public XPCollection<Shipment> AvailableShipments
        {
            get
            {
                if (InvoiceID == null || InvoiceID.CustomerID == null)
                    return new XPCollection<Shipment>(Session, false);

                // Lấy các ShipmentID đã gán cho Invoice hiện tại
                var usedIds = new XPQuery<InvoiceShipment>(Session)
                    .Select(x => x.ShipmentID)                  // ShipmentID là Guid/Oid
                    .ToList();                                  // List<Guid>

                // Xây criteria: cùng Customer, và không nằm trong usedIds
                var and = new GroupOperator(GroupOperatorType.And);
                // Nếu CustomerID là reference (object), so sánh trực tiếp object:
                and.Operands.Add(new BinaryOperator(nameof(Shipment.CustomerID), InvoiceID.CustomerID));

                if (usedIds.Count > 0)
                {
                    and.Operands.Add(
                        new NotOperator(
                            new InOperator(nameof(Shipment.Oid), usedIds.Cast<object>())
                        )
                    );
                }

                return new XPCollection<Shipment>(Session, and);
            }
        }

        //private XPCollection<CustomerAddress> customerAddress = null;
        //public XPCollection<CustomerAddress> CustomerAddressDataSource
        //{
        //    get
        //    {
        //        if (customerAddress == null)
        //        {
        //            customerAddress = new XPCollection<CustomerAddress>(Session);
        //        }
        //        UpdateCustomerAddressList();
        //        return customerAddress;
        //    }
        //}
        //private void UpdateCustomerAddressList()
        //{

        //    if (ShipmentID != null)
        //    {
        //        if(ShipmentID.CustomerID != null)
        //            customerAddress.Criteria = ShipmentID.CustomerID != null ? CriteriaOperator.Parse("CustomerID = ?", ShipmentID.CustomerID) : null;
        //    }

        //}

        private String shipTo;
        [Size(300)]
        public String ShipTo
        {
            get
            {
                if (!IsLoading && !IsSaving && !IsDeleted && String.IsNullOrEmpty(shipTo))
                {
                    if (ShipmentID != null)
                    {
                        if (ShipmentID.CustomerID != null)
                        {

                            shipTo = ShipmentID.CustomerID.Company + Environment.NewLine + ShipmentID.CustomerID.Address;
                        }
                    }
                }
                return shipTo;
            }
            set { shipTo = value; }
        }

    }
    [System.ComponentModel.DefaultProperty("CustPO")]
    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Customer Order Entry")]
    public class InvoiceDetail : BaseObject
    {
        public InvoiceDetail(Session session) : base(session) { }


        private Invoice invoiceID;
        [Association("InvoiceDetail-Invoice", typeof(Invoice))]
        public Invoice InvoiceID
        {
            get { return invoiceID; }
            set { invoiceID = value; }
        }

        public Shipment ShipmentID { get; set; }

        //public PO POID { get; set; }

        //public PODetail PODetailID { get; set; }

        public ShipmentDetail ShipmentDetailID { get; set; }

        public ShipmentDetailPO ShipmentDetailPOID { set; get; }

        protected override void OnDeleted()
        {
            base.OnDeleted();

            if (InvoiceID != null)
            {
                DBAccess _db = new DBAccess();
                int nRst = _db.DeleteInvoiceDetail(InvoiceID.Oid.ToString());
            }

        }
    }
}
