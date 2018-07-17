import * as React from "react";
import {CryptoChart, Point} from "../components/CryptoChart";

export function GetChart(tradeDataPoint: ExchangeAndTrades) {
    // the format of the keys are `exchange/base-sub`
    const sp1 = tradeDataPoint.Meta.split("/");
    const sp2 = sp1[1].split("-");
    return (
        <div>
            <CryptoChart
                Exchange={sp1[0]}
                BaseCurrency={sp2[0]}
                SubCurrency={sp2[1]}
                Width={800}
                Height={460}
                Data={[ tradeDataPoint.Trades ]}/>
        </div>
);
}

export function GetCharts(tradeData: ExchangeAndTrades[]) {
    return tradeData.map(GetChart);
}

export interface ExchangeAndTrades {
    Meta: string,
    Trades: Point[]
}