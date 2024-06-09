using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using System;

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

    enum CurrencyUnit
{
    None,
    K,
    M,
    B,
    T,
    Qa,
    Qi,
    Sx,
    Sp,
    Oc,
    No,
    De,
    Ud,
    Dd,
    Td,
    Qad,
    Qid,
    Sxd,
    Spd,
    Od,
    Nd,
    Vg,
    Uvg,
    Dvg,
    Tvg,
    Qavg,
    Qivg,
    Sxvg,
    Spvg,
    Ocvg,
    Novg,
    Tg,
    Utg,
    Dtg,
    Ttg,
    Qatg,
    Qitg,
    Sxtg,
    Sptg,
    Octg,
    Notg,
    Qag,
    Uqag,
    Dqag,
    Tqag,
    Qaqag,
    Qiqag,
    Sxqag,
    Spqag,
    Ocqag,
    Noqag,
    Qig,
    Uqig,
    Dqig,
    Tqig,
    Qaqig,
    Qiqig,
    Sxqig,
    Spqig,
    Ocqig,
    Noqig,
    Sxg,
    Usxg,
    Dsxg,
    Tsxg,
    Qasxg,
    Qisxg,
    Sxsxg,
    Spsxg,
    Ocsxg,
    Nosxg,
    Spg,
    Uspg,
    Dspg,
    Tspg,
    Qaspg,
    Qispg,
    Sxspg,
    Spspg,
    Ocspg,
    Nospg,
    Ocg,
    Uocg,
    Docg,
    Tocg,
    Qaocg,
    Qiocg,
    Sxocg,
    Spocg,
    Ococg,
    Noocg,
    Nog,
    Unog,
    Dnog,
    Tnog,
    Qanog,
    Qinog,
    Sxnog,
    Spnog,
    Ocnog,
    Nonog,
    C,
    Uc,
    Dc,
    Tc,
    Qac,
    Qic,
    Sxc,
    Spc,
    Occ,
    Noc,
}
}


