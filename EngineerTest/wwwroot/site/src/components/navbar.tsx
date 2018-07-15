import * as React from "react";

export interface NavbarProps {
    loggedIn: boolean
}

export class Navbar extends React.Component<NavbarProps, {}>{
    render() {
        return <nav className="navbar navbar-inverse navbar-fixed-top">
            <div className="container">
                <div className="navbar-header">
                    <button type="button" className="navbar-toggle" data-toggle="collapse"
                            data-target=".navbar-collapse">
                        <span className="sr-only">Toggle navigation</span>
                        <span className="icon-bar"></span>
                        <span className="icon-bar"></span>
                        <span className="icon-bar"></span>
                    </button>
                    <a href={"/"} className="navbar-brand">EngineerTest</a>
                </div>
                <div className="navbar-collapse collapse">
                    <ul className="nav navbar-nav">
                        <li><a href={"/"}>Home</a></li>
                        <li><a href={"/About"}>About</a></li>
                        <li><a href={"/Contact"}>Contact</a></li>
                    </ul>
                </div>
            </div>
        </nav>
    }
}

