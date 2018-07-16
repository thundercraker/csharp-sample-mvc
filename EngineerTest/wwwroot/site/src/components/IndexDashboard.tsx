import * as React from "react";

import {CryptoChart, Point} from "./CryptoChart";

export interface IndexDashboardProps { 
    HasDashboard: boolean,
    NoSetup: boolean,
    TradeData: ExchangeAndTrades[]
}

export interface ExchangeAndTrades {
    Meta: string,
    Trades: Point[]
}

// 'HelloProps' describes the shape of props.
// State is never set so we use the '{}' type.
export class IndexDashboard extends React.Component<IndexDashboardProps, {}> {
    constructor(props: IndexDashboardProps) {
        super(props);
        console.log(props);
        this.state = {};
    }
    
    getChart = (points: Point[], title: string) => {
        // the format of the keys are `exchange/base-sub`
        const sp1 = title.split("/");
        const sp2 = sp1[1].split("-");
        return (
            <div>
                <h3>{sp1[0]} - {sp1[1]}</h3>
                <CryptoChart
                    BaseCurrency={sp2[0]}
                    SubCurrency={sp2[1]}
                    Width={600}
                    Height={400}
                    Data={[ points ]}/>
            </div>
        );
    };

    getCharts = () => this.props.TradeData.map(val => {
        console.log(val);
        return this.getChart(val.Trades, val.Meta);
    });
    
    doRender = () => {
        if ((this.props.HasDashboard)) {
            return (
                <div>
                    <h1>Dashboard</h1>
                    {this.getCharts()}
                </div>);
        } else {
            return (
                <div>
                    <h1>Log in to see a dashboard here!</h1>
                </div>
            );
        }
    };
       
    render() {
        return this.doRender(); 
    }
}