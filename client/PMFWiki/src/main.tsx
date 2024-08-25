import ReactDOM from 'react-dom/client'
import 'bootstrap/dist/css/bootstrap.css'
import { Navigate, RouterProvider, createBrowserRouter } from 'react-router-dom'
import HomePage from './pages/HomePage'
import Proba from './components/Proba'
import '../node_modules/bootstrap/dist/css/bootstrap.min.css'
import Register from './components/Register/Register'
import Login from './components/Login/Login'
import ChceckEmailPage from './pages/CheckEmailPage'
import ConfirmRegisration from './pages/ConfirmRegistration'
import AuthProvider from 'react-auth-kit';
import createStore from 'react-auth-kit/createStore';
import { UserInfo } from './models/UserInfo'
import StartPage from './pages/StartPage'
import { storageService } from './services/StorageService'
import EmailForResetPassword from './components/EmailForResetPassword'
import PasswordChange from './pages/PasswordChange'
import ProfilePage from './pages/ProfilePage'
import SubjectsPage from './pages/SubjectsPage'
import Messages from './pages/Messages'
import React from 'react'
import App from './App'

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
    path: "/email/reset",
    element: <EmailForResetPassword/>
  },
  {
    path: "/poruke-1",
    element: isAuthenticated() ? <Messages/> : <Navigate to="/Prijava"/>,
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
    path: "/promena-lozinke/:resetToken",
    element: <PasswordChange/>
  },
  {
    path: "/pocetna",
    element: isAuthenticated() ? <StartPage/> : <Navigate to="/Prijava"/>,
  },
  {
    path: "/predmeti",
    element: isAuthenticated() ? <SubjectsPage/> : <Navigate to="/Prijava"/>,
  },
  {
    path: "/profilna-strana/:id?",
    element: isAuthenticated() ? <ProfilePage/> : <Navigate to="/Prijava"/>
  }
]);

ReactDOM.createRoot(document.getElementById('root')!).render(
  <RouterProvider router={router}/>
);