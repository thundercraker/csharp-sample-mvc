import * as React from "react";
import {GetCharts, ExchangeAndTrades} from "../charts/Utils";

export interface TradesDashboardProps {
    TradeData: ExchangeAndTrades[]
}

export class TradesDashboard extends React.Component<TradesDashboardProps, {}>{
    constructor(props: TradesDashboardProps) {
        super(props);
        this.state = {}
    }
    
    render() { return (
        <div>
            {GetCharts(this.props.TradeData)}
        </div>
    )}
}