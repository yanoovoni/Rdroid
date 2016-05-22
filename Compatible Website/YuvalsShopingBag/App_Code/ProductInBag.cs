using System;

	public class ProductInBag
	{

        protected int mProdID;  //��� ����
        protected string mProdName;  // �� ����
        protected	decimal	mPrice ;  // price for single unit
		protected	System.Int16 mQuantity;  //����
	//	protected	int		mProdIndex ;
        

		public ProductInBag()
		{
            mProdID = -1;
            mProdName = string.Empty;
			mPrice = -1;  // price for single unit
			mQuantity = 0;
			//mProdIndex = -1;			
		}
        public ProductInBag(ProductInBag product)
        {
            this.mPrice = product.Price;
            this.mProdID = product.ProdID;
            this.mProdName = product.ProdName;
            //this.ProductIndex = p.ProductIndex;
            this.mQuantity = product.Quantity;
		}
		public ProductInBag(int inProdID, string inProdName, decimal inPrice, short inQuantity)
        {
            mProdID = inProdID;
            mProdName = inProdName;
			mQuantity = inQuantity;
			mPrice = inPrice;
		}

        public ProductInBag(int inProdID)
        {  }

        public int ProdID
        {
            get
            {
                return mProdID;
            }
            set
            {
                mProdID = value;
            }
        }

        public string ProdName
        {
            get
            {
                return mProdName;
            }
            set
            {
                mProdName = value;
            }
        }

		public decimal Price
		{
			get 
			{
				return mPrice;
			}
			set
			{
				mPrice = value;
			}
		}

		public System.Int16 Quantity
		{
			get 
			{
				return mQuantity;
			}
			set
			{
				mQuantity = value;
			}
		}

        //public int ProductIndex
        //{
        //    get
        //    {
        //        return mProdIndex;
        //    }
        //    set
        //    {
        //        mProdIndex = value;
        //    }
        //}

        


        


      }

