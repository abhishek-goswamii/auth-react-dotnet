import './App.css'
import SignUP from './Pages/Signup';
import { Routes, Route } from "react-router-dom"
import Login from './Pages/Login';
import DashBoard from './Pages/DashBoard';
import AccounVerificationPage from './Pages/AccounVerificationPage';
import ResetPassword from './Pages/ResetPassword';

function App() {

  return (
    <>
      

      <Routes>
        <Route path="/signup" element={ <SignUP/> } />
        <Route path="/signin" element={ <Login/> } />
        <Route path="/" element={ <DashBoard/> } />
        <Route path="/verify" element={ <AccounVerificationPage/> } />
        <Route path="/reset-password" element={ <ResetPassword/> } />

      </Routes>

 

    </>
  )
}

export default App
