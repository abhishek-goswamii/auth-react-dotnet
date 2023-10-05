/* eslint-disable no-unused-vars */
import * as React from 'react';

import Button from '@mui/material/Button';

import TextField from '@mui/material/TextField';

import { useState } from 'react';
import { baseurl } from '../baseurl';
import Paper from '@mui/material/Paper';
import GoogleLoginComponent from './GoogleLoginComponent';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';


const ResetPassword = () => {

    const navigate = useNavigate()

    const [password, setpassword] = useState("");

    const submit = async () => {
      
        const getTokenFromURL = () => {
          const params = new URLSearchParams(window.location.search);
          return params.get('token');
        }
    
        const token = getTokenFromURL();

        try {
            const response = await axios.post(`${baseurl}/api/reset-password`,{
                "Password":password,
              }, {
                headers: {
                    Authorization: `Bearer ${token}`
                },
              });
            
              alert("Password reset successfully")

        } catch (error) {
            alert("Something went wrong")

            console.log(error)
        }

    }


  return (
    <>

<Paper elevation={3} style={{ display:"flex" , flexDirection:"column" , width:"50vw" , padding:"20px"}} >

<TextField style={{ margin:"10px" }} id="outlined-basic" label="Password" variant="outlined" name='Password' value={password} onChange={(e)=>setpassword(e.target.value)}/>
<Button style={{ margin:"10px" }} variant="outlined"   onClick={submit}>Reset</Button>
<Button style={{ margin:"10px" }} variant="outlined"   onClick={()=>navigate("/signin")}>Go to login page</Button>


</Paper>

    </>
  )
}

export default ResetPassword