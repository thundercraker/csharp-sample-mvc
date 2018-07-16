import * as React from "react";

import {LineChart} from "react-easy-chart";

export interface CryptoCharProps {
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
        <LineChart
            axes
            dataPoints
            grid
            xType={'text'}
            margin={{top: 0, right: 0, bottom: 30, left: 30}}
            axisLabels={{x: 'Time', y: this.props.SubCurrency}}
            yAxisOrientRight
            width={this.props.Width}
            height={this.props.Height}
            data={this.props.Data}
        />);
    }
}