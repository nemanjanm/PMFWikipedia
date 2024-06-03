import React from 'react'
import ReactDOM from 'react-dom/client'
import 'bootstrap/dist/css/bootstrap.css'
import { RouterProvider, createBrowserRouter } from 'react-router-dom'
import HomePage from './components/MenuBar/MenuBar'
import Proba from './components/Proba'
import '../node_modules/bootstrap/dist/css/bootstrap.min.css'

const router = createBrowserRouter([
  {
    path: "/",
    element: <HomePage/>,
    children: [
      {    
        path: "/Wikipedia",
        element: <Proba/>
      },
      // {    
      //   path: "/Registracija",
      //   element: <Proba/>
      // },
      // {    
      //   path: "/Prijava",
      //   element: <Proba/>
      // }
    ]
  }
]);

ReactDOM.createRoot(document.getElementById('root')!).render(
  
  <React.StrictMode>
    <RouterProvider router={router}></RouterProvider>
  </React.StrictMode>,
)
