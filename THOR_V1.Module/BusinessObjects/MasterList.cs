using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using THOR_V1.Module.DatabaseUpdate;

namespace THOR_V1.Module.BusinessObjects
{
    [RuleCriteria("Pack's width is greater than 0", DefaultContexts.Save, "W_Cm > 0", "Pack's width is greater than 0", SkipNullOrEmptyValues = false)]
    [RuleCriteria("Pack's depth is greater than 0", DefaultContexts.Save, "D_Cm > 0", "Pack's depth is greater than 0", SkipNullOrEmptyValues = false)]
    [RuleCriteria("Pack's height is greater than 0", DefaultContexts.Save, "H_Cm > 0", "Pack's height is greater than 0", SkipNullOrEmptyValues = false)]
    [System.ComponentModel.DefaultProperty("PartID")]
    [DefaultClassOptions, ImageName("BO_Product")]
    [NavigationItem(GroupName = "Master Lists")]
    [DefaultListViewOptions(true, NewItemRowPosition.None)]
    //  [Appearance("HidePhoneForNonAdministrators", TargetItems = "ItemID", Criteria = "IsCurrentUserInRole('R&D Data Audit')", Visibility = ViewItemVisibility.Hide, Context = "Any")]
    public class ItemPack : BaseObject
    {
        private Item itemID;
        //    private PackType packTypeID;
        private Part oldpartID;
        private Part partID;
        private String comments;
        private string remarks;
        private Double w_Cm;
        private Double d_Cm;
        private Double h_Cm;
        private Double oldw_Cm = 0;
        private Double oldd_Cm = 0;
        private Double oldh_Cm = 0;
        private Double packedWeight;
        private bool isFinal;
        private Double netWeight;
        private bool isAddNew = false;

        public ItemPack(Session session) : base(session)
        {
            oldpartID = partID;
            oldw_Cm = w_Cm;
            oldd_Cm = d_Cm;
            oldh_Cm = h_Cm;
        }

        protected override void OnLoaded()
        {
            base.OnLoaded();
            oldpartID = partID;
            oldw_Cm = w_Cm;
            oldd_Cm = d_Cm;
            oldh_Cm = h_Cm;
        }

        public override void AfterConstruction()
        {
            isAddNew = true;
            base.AfterConstruction();
        }

        [Association("Item-ItemPack", typeof(Item))]
        public Item ItemID
        {
            get { return itemID; }
            set { itemID = value; }
        }
        //[Association("ItemPack-PackType", typeof(PackType))]
        //public PackType PackTypeID
        //{
        //    get { return packTypeID; }
        //    set { packTypeID = value; }
        //}
        [Association("ItemPack-Part", typeof(Part))]
        public Part PartID
        {
            get { return partID; }
            set { partID = value; }
        }
        [Size(200)]
        public String Remarks
        {
            get { return remarks; }
            set { remarks = value; }
        }

        [Size(200)]
        public String Comments
        {
            get { return comments; }
            set { comments = value; }
        }
        [Persistent]
        // [Custom("DisplayFormat", "###,###.#")]
        [Custom("EditMask", "###,##0.###")]
        public Double W_Cm
        {
            get { return w_Cm; }
            set { w_Cm = value; }
        }
        [Persistent]
        // [Custom("DisplayFormat", "###,###.#")]
        [Custom("EditMask", "###,##0.###")]
        public Double D_Cm
        {
            get { return d_Cm; }
            set { d_Cm = value; }
        }
        [Persistent]
        // [Custom("DisplayFormat", "###,###.#")]
        [Custom("EditMask", "###,##0.###")]
        public Double H_Cm
        {
            get { return h_Cm; }
            set { h_Cm = value; }
        }
        public String InchWxDxH
        { // [Inch L/W/H] = ([L cm] / 2.54 * 100 \ 1) / 100 & " x " & ([W cm] / 2.54 * 100 \ 1) / 100 & " x " & ([H cm] / 2.54 * 100 \ 1) / 100

            get
            {

                double winch = W_Cm / 25.4;
                double dinch = D_Cm / 25.4;
                double hinch = H_Cm / 25.4;
                return winch.ToString("0.##") + " x " + dinch.ToString("0.##") + " x " + hinch.ToString("0.##");
            }
        }

        private Double packedCBM;

        // [Custom("DisplayFormat", "###,###.#")]
        [Custom("EditMask", "###,##0.###")]
        public Double PackedCBM
        {
            get
            {
                //return (Double)W_Cm * D_Cm * H_Cm / 1000000;
                //return (Double)W_Cm * D_Cm * H_Cm / 1000000000;
                return packedCBM;

            }
            set { packedCBM = value; }
        }
        //  [Custom("DisplayFormat", "###,###.#")]
        [Custom("EditMask", "###,##0.#")]
        public Double PackedWeight
        {
            get { return packedWeight; }
            set { packedWeight = value; }
        }
        public Double NetWeight
        {
            get { return netWeight; }
            set { netWeight = value; }
        }
        public bool IsFinal
        {
            get { return isFinal; }
            set { isFinal = value; }
        }

        private DateTime finalDate;
        public DateTime FinalDate
        {
            get { return finalDate; }
            set { finalDate = value; }
        }

        private String trackingLog;
        [Size(1000)]
        public String TrackingLog
        {
            get { return trackingLog; }
            set { trackingLog = value; }
        }



        protected override void OnSaving()
        {
            if (isFinal == true && finalDate == DateTime.MinValue)
                finalDate = DateTime.Now;

            StringBuilder strAdd = new StringBuilder();

            StringBuilder strBuild = new StringBuilder();

            if (!isAddNew)
            {
                if (partID != null && oldpartID != null)
                {
                    if (oldpartID != partID)
                    {
                        strAdd.AppendLine("Change content: PartID was changed from " + oldpartID != null ? oldpartID.PartName : "" + " to " + partID.PartName);
                    }
                }

                if (d_Cm != 0)
                {
                    if (d_Cm != oldd_Cm)
                        strAdd.AppendLine("Change content: D_Cm was changed from " + oldd_Cm.ToString() + " to " + d_Cm.ToString());
                }

                if (w_Cm != 0)
                {
                    if (w_Cm != oldw_Cm)
                        strAdd.AppendLine("Change content: W_Cm was changed " + oldw_Cm.ToString() + " to " + w_Cm.ToString());
                }

                if (h_Cm != 0)
                {
                    if (h_Cm != oldh_Cm)
                        strAdd.AppendLine("Change content: H_Cm was changed " + oldh_Cm.ToString() + "  to " + h_Cm.ToString());
                }

                if (strAdd.Length != 0)
                {
                    strBuild.AppendLine("ItemPacked was changed by : " + SecuritySystem.CurrentUserName);
                    strBuild.AppendLine("Last Update : " + DateTime.Now.ToShortDateString());
                    strBuild.AppendLine(strAdd.ToString());
                }
            }
            else
            {
                strBuild.AppendLine("ItemPacked was Created by : " + SecuritySystem.CurrentUserName);
                strBuild.AppendLine("Create Date : " + DateTime.Now.ToShortDateString());
            }

            if (!String.IsNullOrEmpty(trackingLog))
            {
                strBuild.AppendLine(trackingLog);
            }

            if (strBuild.Length != 0)
            {
                trackingLog = strBuild.ToString();
            }

            if (this.ItemID != null)
            {
                DBAccess bs = new DBAccess();
                int nRst = bs.UpdateItemPackToItem(this.ItemID.Oid.ToString());
            }

            base.OnSaving();


        }
    }
}
