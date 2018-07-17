import * as React from "react";

import {ExchangeAndTrades, GetCharts} from "../charts/Utils";

export interface IndexDashboardProps { 
    HasDashboard: boolean,
    NoSetup: boolean,
    TradeData: ExchangeAndTrades[]
}



export class IndexDashboard extends React.Component<IndexDashboardProps, {}> {
    
    constructor(props: IndexDashboardProps) {
        super(props);
        console.log(props);
        this.state = {};
    }
    
    doRender = () => {
        if ((this.props.HasDashboard)) {
            return (
                <div>
                    <h1>Dashboard</h1>
                    {GetCharts(this.props.TradeData)}
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