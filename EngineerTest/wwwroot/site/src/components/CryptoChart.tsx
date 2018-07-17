import * as React from "react";

import {LineChart} from "react-easy-chart";

export interface CryptoCharProps {
    Exchange: string
    BaseCurrency: string, 
    SubCurrency: string, 
    Width: number,
    Height: number,
    Data: Point[][]
}

export interface Point {
    x: any,
    y : any 
}

export class CryptoChart extends React.Component<CryptoCharProps, {}>{
    constructor(props: CryptoCharProps) {
        super(props);
        //const initialWidth = window.innerWidth > 0 ? window.innerWidth : 500;
        this.state = {};
    }
    
    render() {
        return (
            <div>
                <h3>{this.props.Exchange} {this.props.BaseCurrency}/{this.props.SubCurrency}</h3>
                <LineChart
                    axes
                    dataPoints
                    grid
                    xType={'text'}
                    margin={{top: 60, right: 60, bottom: 60, left: 60}}
                    axisLabels={{x: 'Time', y: this.props.SubCurrency + " price"}}
                    yAxisOrientRight
                    width={this.props.Width}
                    height={this.props.Height}
                    data={this.props.Data}
                />
            </div>);
    }
}