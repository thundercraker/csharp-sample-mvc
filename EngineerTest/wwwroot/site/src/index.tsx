import * as React from "react";
import * as ReactDOM from "react-dom";

import {App} from "./components/app";
import {Navbar} from "./components/navbar";

ReactDOM.render(
    <Navbar loggedIn={false} />,
    document.getElementById("react-navbar")
);

var root = document.getElementById("react-app");
if (root) {
    ReactDOM.render(
        <App/>, root
    );
}