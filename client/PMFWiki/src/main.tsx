import ReactDOM from 'react-dom/client'
import 'bootstrap/dist/css/bootstrap.css'
import { RouterProvider, createBrowserRouter } from 'react-router-dom'
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

export const store = createStore<UserInfo>({
  authName: "_auth",
  authType: "cookie",
  cookieDomain: window.location.hostname,
  cookieSecure: false
})
const router = createBrowserRouter([
  {
    path: "/",
    element: <HomePage/>,
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
  }
]);

ReactDOM.createRoot(document.getElementById('root')!).render(
    
  <AuthProvider store={store}>
      <RouterProvider router={router}></RouterProvider>
  </AuthProvider>
)
