import * as React from "react";

export interface NavbarProps {
    userName: string,
    requestVerificationToken: string
}

export class Navbar extends React.Component<NavbarProps, {}>{
    constructor(props: NavbarProps) {
        super(props);
        console.log(props);
        this.state = {};
    }
    
    extraUserLinks = () => {
        if (this.props.userName) {
            return (
                <li><a href={'/Trades'}>Trades</a></li>
            )
        }
        return;
    };
    
    userControls = () => {
        if (this.props.userName) {
            return (
                <form action={'Account/Logout'} method="post" id="logoutForm"
                      className="navbar-right">
                    <input name={'__RequestVerificationToken'} 
                           type={'hidden'}
                           value={this.props.requestVerificationToken} />
                    <ul className="nav navbar-nav navbar-right">
                        <li>
                            <a href={'Manage'} title="Manage">Hello {this.props.userName}!</a>
                        </li>
                        <li>
                            <button type="submit" className="btn btn-link navbar-btn navbar-link">Log out</button>
                        </li>
                    </ul>
                </form>
            );
        } else {
            return (
                <ul className="nav navbar-nav navbar-right">
                    <li><a href={"Account/Register"}>Register</a></li>
                    <li><a href={"Account/Login"}>Log in</a></li>
                </ul>
            );
        }
    };
    
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
                        <li><a href={'/'}>Home</a></li>
                        {this.extraUserLinks()}
                        <li><a href={'/About'}>About</a></li>
                        <li><a href={'/Contact'}>Contact</a></li>
                    </ul>
                    {this.userControls()}
                </div>
            </div>
        </nav>
    }
}

