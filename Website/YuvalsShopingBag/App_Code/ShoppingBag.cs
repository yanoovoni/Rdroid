using System;
using System.Collections;
using System.Data;

public class ShoppingBag
{
    ArrayList mProducts;

    public ShoppingBag()
    {
        mProducts = new ArrayList();
    }

 
    public ArrayList Products
    {
        get
        {
            return mProducts;
        }
    }

    public int Length
    {
        get
        {
            return mProducts.Count;
        }
    }


    public decimal GetFinalPrice()
    {
        decimal sum = 0;
        //����� ���� �� �� ������� ���
        for (int i = 0; i < mProducts.Count; i++)
        {
            sum += ((ProductInBag)mProducts[i]).Quantity * ((ProductInBag)mProducts[i]).Price;
        }
        return sum;
    }
    

    private int SearchProduct(int prodID)
    {
        for (int i = 0; i < mProducts.Count; i++)
        {
            if (((ProductInBag)mProducts[i]).ProdID == prodID) return i;
        }
        return -1;
    }

    // Adds a product to the products list.
    // returns the result of the inserting action (true - everything was alright).
    public void AddProduct(ProductInBag inProduct)
    {
        //����� ���� ��� .�� ���� ��� ��� ���� �� ����� �1  
        int index = SearchProduct(inProduct.ProdID);
        if (index == -1)
        {
            mProducts.Add(inProduct);
        }
        else
        {
            ((ProductInBag)mProducts[index]).Quantity++;
        }
    }

    public void DeleteProduct(ProductInBag inProduct)
    {
        //���� ���� ����
        int index = SearchProduct(inProduct.ProdID);
        if (index != -1)
        {
            mProducts.RemoveAt(index);
        }
    }

    public void UpdateProduct(ProductInBag inProduct)
    {
        //����� ���� �� ���� ���
        int index = SearchProduct(inProduct.ProdID);
        if (index != -1)
        {

            ((ProductInBag)mProducts[index]).Quantity = inProduct.Quantity;
        }
    }

  
}