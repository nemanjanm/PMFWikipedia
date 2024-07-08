import ReactDOM from 'react-dom/client'
import 'bootstrap/dist/css/bootstrap.css'
import { Navigate, RouterProvider, createBrowserRouter } from 'react-router-dom'
import HomePage from './pages/HomePage'
import Proba from './components/Proba'
import '../node_modules/bootstrap/dist/css/bootstrap.min.css'
import Register from './components/Register/Register'
import Login from './components/Login/Login'
import ChceckEmailPage from './pages/CheckEmail/CheckEmailPage'
import ConfirmRegisration from './pages/ConfirmRegistration'
import AuthProvider from 'react-auth-kit';
import createStore from 'react-auth-kit/createStore';
import { UserInfo } from './models/UserInfo'
import StartPage from './pages/StartPage'
import { storageService } from './components/StorageService'

export const store = createStore<UserInfo>({
  authName: "_auth",
  authType: "cookie",
  cookieDomain: window.location.hostname,
  cookieSecure: false
})

const isAuthenticated = () => {
  return storageService.getToken();
}

const router = createBrowserRouter([
  {
    path: "/",
    element: isAuthenticated() ? <Navigate to = "/pocetna" /> : <HomePage/>,
    children: [
      {    
        path: "/Wikipedia",
        element: <Proba/>
      },
      {    
        path: "/Registracija",
        element: <Register/>
      },
      {    
        path: "/Prijava",
        element: <Login/>
      }
    ]
  },
  {
    path: "/email",
    element: <ChceckEmailPage/>
  },
  {
    path: "/confirm/:registrationToken",
    element: <ConfirmRegisration/>
  },
  {
    path: "/pocetna",
    element: isAuthenticated() ? <StartPage/> : <Navigate to="/Prijava"/>
  }
]);

ReactDOM.createRoot(document.getElementById('root')!).render(
    
  <AuthProvider store={store}>
      <RouterProvider router={router}></RouterProvider>
  </AuthProvider>
)
