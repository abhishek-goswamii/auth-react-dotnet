/* eslint-disable no-unused-vars */
import * as React from 'react';

import Button from '@mui/material/Button';

import TextField from '@mui/material/TextField';

import { useState } from 'react';
import axios from 'axios';
import { baseurl } from '../baseurl';
import Paper from '@mui/material/Paper';
import GoogleLoginComponent from './GoogleLoginComponent';
import { useNavigate } from 'react-router-dom';
import { Typography } from '@mui/material';



export default function SignUP() {

    const [name, setname] = useState("");
    const [email, setemail] = useState("");
    const [password, setpassword] = useState("");




  const navigate = useNavigate()  
    const handleSubmit = async () => {
 
      try {
        const response = await axios.post(`${baseurl}/api/register`,{
          "Name":name,
          "Email":email,
          "Password":password,
          "Address":"none"
        }, {
          headers: {
            "Content-Type": "application/json",
          },
        });
        
        console.log("login success",response)
        alert("a account verification is sent to your email, click on it to veify ")

      } catch (error) {
        alert("something went wrong")
        console.log(error)
      }



    };

  const GoogleSigninsuccess = (response)=>{
      console.log(response.profileObj.name)
  }

  return (
      
      <Paper elevation={3} style={{ display:"flex" , flexDirection:"column" , width:"50vw" , padding:"20px"}} >

      <TextField style={{ margin:"10px" }} id="outlined-basic" label="Name" variant="outlined" name='Name' value={name} onChange={(e)=>setname(e.target.value)}/>
      <TextField style={{ margin:"10px" }} id="outlined-basic" label="Email" variant="outlined" name='Email' value={email} onChange={(e)=>setemail(e.target.value)} error={email && !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)} helperText={email && !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email) ? "Email should be valid" : ""}/>
      <TextField style={{ margin:"10px" }} id="outlined-basic" label="Password" variant="outlined" name='Password' value={password} onChange={(e)=>setpassword(e.target.value)}/>
      <Button style={{ margin:"10px" }} variant="outlined"   onClick={handleSubmit}>Signup</Button>
      <Button style={{ margin:"10px" }} variant="outlined"   onClick={()=>navigate('/signin')}>go to login</Button>
      
      <GoogleLoginComponent loginSuccess={GoogleSigninsuccess}/>
      </Paper>

  );
}

