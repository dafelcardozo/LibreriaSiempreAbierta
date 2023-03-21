import React, { Component } from 'react';
import { Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';
import 'mdb-ui-kit/css/mdb.min.css';
import "@fortawesome/fontawesome-free/css/all.min.css";
import '@fortawesome/fontawesome-svg-core/styles.css'

export class NavMenu extends Component {
    static displayName = NavMenu.name;

    constructor(props) {
        super(props);

        this.toggleNavbar = this.toggleNavbar.bind(this);
        this.state = {
            collapsed: true
        };
    }

    toggleNavbar() {
        this.setState({
            collapsed: !this.state.collapsed
        });
    }

    render() {
        return (
            <header>
                <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" container light>
                    <NavbarBrand tag={Link} to="/">Avenue17</NavbarBrand>
                    <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
                    <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed} navbar>
                        <ul className="navbar-nav flex-grow">
                            <NavItem>
                                <NavLink tag={Link} className="text-dark" to="/">Home</NavLink>
                            </NavItem>
                            <NavItem>
                                <NavLink tag={Link} className="text-dark" to="/editorials">Editorials</NavLink>
                            </NavItem>
                            <NavItem>
                                <NavLink tag={Link} className="text-dark" to="/authors">Authors</NavLink>
                            </NavItem>
                            <NavItem>
                                <NavLink tag={Link} className="text-dark" to="/search">Browse books</NavLink>
                            </NavItem>
                        </ul>
                    </Collapse>
                </Navbar>
            </header>
        );
    }
    renderAlternative() {
        return alternative();
    }
}


const alternative = () => (
<header>
    <nav className="navbar navbar-expand-lg navbar-dark bg-dark">
        <div className="container-fluid">
            <button className="navbar-toggler" type="button" data-mdb-toggle="collapse"
                data-mdb-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false"
                aria-label="Toggle navigation">
                <i className="fas fa-bars"></i>
            </button>

            <div className="collapse navbar-collapse" id="navbarSupportedContent">
                <a className="navbar-brand mt-2 mt-lg-0" href="/" >
                    <img src="Logo_Lite_Thinking_Sin_Fondo_1.png" alt="Lite Thinking" width="60" />
                </a>
                <ul className="navbar-nav me-auto mb-2 mb-lg-0">
                    <li className="nav-item">
                        <a className="nav-link" href="Felipe Cardozo - English CV 2023.pdf">Hoja de vida</a>
                    </li>
                </ul>
            </div>

            <div className="d-flex align-items-center">
                <a className="text-reset me-3" href="#">
                    <i className="fas fa-shopping-cart"></i>
                </a>

                <div className="dropdown">
                    <a className="text-reset me-3 dropdown-toggle hidden-arrow" href="#" id="navbarDropdownMenuLink" role="button"
                        data-mdb-toggle="dropdown" aria-expanded="false">
                        <i className="fas fa-bell"></i>
                        <span className="badge rounded-pill badge-notification bg-danger">1</span>
                    </a>
                    <ul className="dropdown-menu dropdown-menu-end" aria-labelledby="navbarDropdownMenuLink">
                        <li>
                            <a className="dropdown-item" href="#">Some news</a>
                        </li>
                        <li>
                            <a className="dropdown-item" href="#">Another news</a>
                        </li>
                        <li>
                            <a className="dropdown-item" href="#">Something else here</a>
                        </li>
                    </ul>
                </div>
                <div className="dropdown">
                    <a className="dropdown-toggle d-flex align-items-center hidden-arrow" href="#" id="navbarDropdownMenuAvatar"
                        role="button" data-mdb-toggle="dropdown" aria-expanded="false">ABC</a>
                    <ul className="dropdown-menu dropdown-menu-end" aria-labelledby="navbarDropdownMenuAvatar">
                        <li>
                            <a className="dropdown-item" href="#">Mi hoja de vida</a>
                        </li>
                        <li>
                            <a className="dropdown-item" href="#">Settings</a>
                        </li>
                        <li>
                            <a className="dropdown-item" href="#">Logout</a>
                        </li>
                    </ul>
                </div>
            </div>
        </div>
    </nav>
    </header >
);
