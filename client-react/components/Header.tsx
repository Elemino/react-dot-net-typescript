import * as React from "react";
import { Link, Redirect } from 'react-router-dom';
import { RouteComponentProps } from "react-router";
import { RoutePaths } from './Routes';
import AuthService from '../services/Auth';
import AuthStore from '../stores/Auth';

let authService = new AuthService();

export class Header extends React.Component<RouteComponentProps<any>, any> {
    signOut() {
        authService.signOut();
        this.props.history.push(RoutePaths.SignIn, { signedOut: true });
    }

    render() {
        const search = this.props.location.search;
        const params = new URLSearchParams(search);

        return <nav className="navbar navbar-expand-lg navbar-dark bg-dark fixed-top">
            <button className="navbar-toggler navbar-toggler-right" type="button" data-toggle="collapse" data-target="#navbarsExampleDefault"
                aria-controls="navbarsExampleDefault" aria-expanded="false" aria-label="Toggle navigation">
                <span className="navbar-toggler-icon"></span>
            </button>
            <a className="navbar-brand" href="#">Financial Dashboard</a>
            <div className="collapse navbar-collapse" id="navbarsExampleDefault">
                <button className="btn btn-outline-warning my-2 my-sm-0" type="button" onClick={() => this.signOut()}>Sign out</button>
            </div>
        </nav>;
    }
}
