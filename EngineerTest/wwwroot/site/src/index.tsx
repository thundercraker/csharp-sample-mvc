import * as React from "react";
import * as ReactDOM from "react-dom";

import {IndexDashboard} from "./components/IndexDashboard";
import {Navbar} from "./components/Navbar";
import {ExchangeAndTrades} from "./charts/Utils";
import {TradesDashboard} from "./components/TradesDashbord";

// get user information

const appDataEl = document.getElementById("app-data");
function getUserName() {
    return appDataEl.dataset["username"];
}
function getRequestVerificationToken() {
    return appDataEl.getElementsByTagName("input")[0].value;
}
ReactDOM.render(
    <Navbar userName={getUserName()} requestVerificationToken={getRequestVerificationToken()} />,
    document.getElementById("react-navbar")
);

// index page
const indexRoot = document.getElementById("react-app-index");
const tradesRoot = (indexRoot) ? null : document.getElementById("react-app-trades");
if (indexRoot) {
    let obj = JSON.parse(indexRoot.dataset["model"]) || {};
    console.log(obj);
    ReactDOM.render(
        <IndexDashboard 
            NoSetup={obj.NoSetup || true}
            HasDashboard={obj.HasDashboard || false}
            TradeData={obj.Data as ExchangeAndTrades[] || []}
        />, indexRoot
    );
} else if (tradesRoot) {
    let obj = JSON.parse(tradesRoot.dataset["model"]) || {};
    console.log(obj);
    ReactDOM.render(
        <TradesDashboard TradeData={obj.TradeData as ExchangeAndTrades[] || []}/>,
        tradesRoot
    )
}