import { Button, Typography } from '@mui/material';
import React, { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import { baseurl } from '../baseurl';
import { googleLogout } from '@react-oauth/google';
import TextField from '@mui/material/TextField';


const DashBoard = () => {
    

    const navigate = useNavigate()
    const [profile, setProfile] = useState(null);
    const [newPassword, setnewPassword] = useState();
    
    useEffect(() => {
       
        const token = localStorage.getItem('token');
        if (!token) {
          navigate('/signin');
        } else {
          axios.get(`${baseurl}/api/profile`, {
            headers: {
              Authorization: `Bearer ${token}`
            }
          }).then(response => {
            setProfile(response.data);
          }).catch(error => {
            console.error(error);
          });
        }
        
    }, []);


    const sendresetlink = async () => {
      

      try {

        const token = localStorage.getItem('token');
        
        const response = await axios.get(`${baseurl}/api/forgot-password`,{
          headers: {
            Authorization: `Bearer ${token}`
          },
        });
        
      
        alert("a link is reset password sent to your email")

      } catch (error) {
        alert("something went wrong")
        console.log(error)
      }

    }








    return (
    <>
    
    <Button variant="contained" onClick={() => {
        googleLogout();
        localStorage.removeItem('token');
        navigate('/signin');
    }}>Logout</Button>

    {profile && (
      <>
        <div>Name: {profile.name}</div>
        <div>Email: {profile.email}</div>
        <div>Address: {profile.address}</div>
      </>
    )}

      <Typography>reset password from here</Typography>
      <TextField style={{ margin:"10px" }} id="outlined-basic" label="Password" variant="outlined" name='new Password' value={newPassword} onChange={(e)=>setnewPassword(e.target.value)}/>
      <Button style={{ margin:"10px" }} variant="outlined"   onClick={sendresetlink}>click to send reset password link</Button>

    </>
  )
}

export default DashBoard