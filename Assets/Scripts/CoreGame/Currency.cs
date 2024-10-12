using System;
using System.Collections.Generic;
using System.Numerics;

public class Currency
{
    public static string DisplayCurrency(string gold)
    {
        int goldLength = gold.Length;
        if (goldLength < 4)
            return gold.ToString();

        //Display currency
        //1,00k = 1000 , 11,00k = 11000, 111,00k = 111000
        string currency = string.Empty;
        char firstChar = gold[0];
        char secondChar = gold[1];
        char thirdChar = gold[2];

        int div = goldLength % 3;
        int unit = (goldLength - 1) / 3;
        string goldUnit = Enum.GetName(typeof(CurrencyUnit), unit);
        switch (div)
        {
            case 0:
                currency = $"{firstChar}{secondChar}{thirdChar}{goldUnit}";
                break;
            case 1:
                currency = $"{firstChar}.{secondChar}{thirdChar}{goldUnit}";
                break;
            case 2:
                currency = $"{firstChar}{secondChar}.{thirdChar}{goldUnit}";
                break;
        }

        return currency;
    }

    public static string DisplayCurrency(double gold)
    {
        if (gold < 1000)
        {
            return gold.ToString("F1");
        }

        BigInteger goldInt = (BigInteger)gold;
        string goldString = goldInt.ToString();
        int goldLength = goldString.Length;

        string currency = string.Empty;
        char firstChar = goldString[0];
        char secondChar = goldString[1];
        char thirdChar = goldString[2];

        int div = goldLength % 3;
        int unit = (goldLength - 1) / 3;
        string goldUnit = Enum.GetName(typeof(CurrencyUnit), unit);
        switch (div)
        {
            case 0:
                currency = $"{firstChar}{secondChar}{thirdChar}{goldUnit}";
                break;
            case 1:
                currency = $"{firstChar}.{secondChar}{thirdChar}{goldUnit}";
                break;
            case 2:
                currency = $"{firstChar}{secondChar}.{thirdChar}{goldUnit}";
                break;
        }

        return currency;
    }
	public static double ConvertCurrencyToDouble(string currency)
	{
		int index = 0;
		while (index < currency.Length && (char.IsDigit(currency[index]) || currency[index] == '.'))
		{
			index++;
		}

		// Extract the number and unit parts
		string numberPart = currency.Substring(0, index);
		string unitPart = currency.Substring(index);

		// Parse the number part to a double
		if (!double.TryParse(numberPart, out double number))
		{
			throw new ArgumentException("Invalid currency format");
		}

	
		int unitPower = GetUnitPower(unitPart);

		double result = number * Math.Pow(1000, unitPower);

		return result;
	}
	private static int GetUnitPower(string unit)
	{
		// Define the unit mapping from the CurrencyUnit enum
		Dictionary<string, int> unitMap = new Dictionary<string, int>()
	{
		{"K", 1}, {"M", 2}, {"B", 3}, {"T", 4}, {"aa", 5}, {"ab", 6}, {"ac", 7}, {"ad", 8},
		{"ae", 9}, {"af", 10}, {"ag", 11}, {"ah", 12}, {"ai", 13}, {"aj", 14}, {"ak", 15},
		{"al", 16}, {"am", 17}, {"an", 18}, {"ao", 19}, {"ap", 20}, {"aq", 21}, {"ar", 22},
		{"as", 23}, {"at", 24}, {"au", 25}, {"av", 26}, {"aw", 27}, {"ax", 28}, {"ay", 29},
		{"az", 30}, {"ba", 31}, {"bb", 32}, {"bc", 33}, {"bd", 34}, {"be", 35}, {"bf", 36},
		{"bg", 37}, {"bh", 38}, {"bi", 39}, {"bj", 40}, {"bk", 41}, {"bl", 42}, {"bm", 43},
		{"bn", 44}, {"bo", 45}, {"bp", 46}, {"bq", 47}, {"br", 48}, {"bs", 49}, {"bt", 50},
		{"bu", 51}, {"bv", 52}, {"bw", 53}, {"bx", 54}, {"by", 55}, {"bz", 56}, {"ca", 57},
		{"cb", 58}, {"cc", 59}, {"cd", 60}, {"ce", 61}, {"cf", 62}, {"cg", 63}, {"ch", 64}
    };
		if (unitMap.ContainsKey(unit))
		{
			return unitMap[unit];
		}
		else
		{
			throw new ArgumentException("Invalid unit in currency string");
		}
	}
}

enum CurrencyUnit
{
    None,
    K,
    M,
    B,
    T,
    aa,
    ab,
    ac,
    ad,
    ae,
    af,
    ag,
    ah,
    ai,
    aj,
    ak,
    al,
    am,
    an,
    ao,
    ap,
    aq,
    ar,
    aS,
    at,
    au,
    av,
    aw,
    ax,
    ay,
    az,
    ba,
    bb,
    bc,
    bd,
    be,
    bf,
    bg,
    bh,
    bi,
    bj,
    bk,
    bl,
    bm,
    bn,
    bo,
    bp,
    bq,
    br,
    bs,
    bt,
    bu,
    bv,
    bw,
    bx,
    by,
    bz,
    ca,
    cb,
    cc,
    cd,
    ce,
    cf,
    cg,
    ch,
    ci,
    cj,
    ck,
    cl,
    cm,
    cn,
    co,
    cp,
    cq,
    cr,
    cs,
    ct,
    cu,
    cv,
    cw,
    cx,
    cy,
    cz,
    da,
    db,
    dc,
    dd,
    de,
    df,
    dg,
    dh,
    di,
    dj,
    dk,
    dl,
    dm,
    dn,
    dO,
    dp,
    dq,
    dr,
    ds,
    dt,
    du,
    dv,
    dw,
    dx,
    dy,
    dz,
}


