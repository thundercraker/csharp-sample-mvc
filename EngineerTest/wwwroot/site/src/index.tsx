import * as React from "react";
import * as ReactDOM from "react-dom";

import {App} from "./components/app";
import {Navbar} from "./components/navbar";

// get user information
function getUserName() {
    const appDataEl = document.getElementById("app-data");
    return appDataEl.dataset["username"];
}

ReactDOM.render(
    <Navbar userName={getUserName()} />,
    document.getElementById("react-navbar")
);

var root = document.getElementById("react-app");
if (root) {
    ReactDOM.render(
        <App/>, root
    );
}