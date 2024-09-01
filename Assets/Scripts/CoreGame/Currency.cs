using System;
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


