import RestUtilities from './RestUtilities';
import AuthStore from '../stores/Auth';

interface IAuthResponse {
    token: string;
}

export default class Auth {
    static isSignedIn(): boolean {
        return !!AuthStore.getToken();
    }

    signIn(username: string, password: string) {
        return RestUtilities.post<IAuthResponse>('/api/auth/login',
        `username=${username}&password=${password}&grant_type=password`)
        .then((response) => {
            if (!response.is_error) {
                AuthStore.setToken(response.content.token);
            }
            return response;
        });
    }

    register(username: string, password: string, firstname: string, lastname: string, email: string) {
        return RestUtilities.post<IAuthResponse>('/api/auth/register',
        `username=${username}&password=${password}&firstname=${firstname}&lastname=${lastname}&email=${email}`)
        .then((response) => {
            if (!response.is_error) {
                AuthStore.setToken(response.content.token);
            }
            return response;
        });
    }

    confirm(token: string): Promise<boolean> {
        return RestUtilities.post('/api/auth/confirm', { token: token })
            .then((response) => {
                return true;
            }).catch((err) => {
                console.log(err);
                return false;
            });
    }

    signOut(): void {
        AuthStore.removeToken();
    }
}
