using System;

	public class ProductInBagIn:Product
	{
		
		protected	decimal	mPrice ;  // price for single unit
		protected	System.Int16 mQuantity;  //כמות
	//	protected	int		mProdIndex ;
        

		public ProductInBagIn()
		{
            //mProdID = -1;
            //mProdName = string.Empty;
			mPrice = -1;  // price for single unit
			mQuantity = 0;
			//mProdIndex = -1;			
		}
        public ProductInBagIn(ProductInBagIn product): base(product)
		{
            this.mPrice = product.Price;
            //this.mProdID=p.ProdID;
            //this.mProdName=p.ProdName;
			//this.ProductIndex=p.ProductIndex;
            this.mQuantity = product.Quantity;
		}
		public ProductInBagIn(int inProdID, string inProdName, decimal inPrice, short inQuantity):base(inProdID,inProdName)
		{
            //mProdID = inProdID;
            //mProdName = inProdName;
			mQuantity = inQuantity;
			mPrice = inPrice;
		}

        public ProductInBagIn(int inProdID):base(inProdID)
        {  }

        //public int ProdID
        //{
        //    get
        //    {
        //        return mProdID;
        //    }
        //    set
        //    {
        //        mProdID = value;
        //    }
        //}

        //public string ProdName
        //{
        //    get 
        //    {
        //        return mProdName;
        //    }
        //    set
        //    {
        //        mProdName = value;
        //    }
        //}

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

