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
import jwtDecode from 'jwt-decode';


const Login = () => {

    const [email, setemail] = useState("");
    const [password, setpassword] = useState("");
    
    const [user, setuser] = useState({});

    const naviagte = useNavigate()

    const handleSubmit = async () => {
      
      try {
        const response = await axios.post(`${baseurl}/api/login`,{
          "Email":email,
          "Password":password,
        }, {
          headers: {
            "Content-Type": "application/json",
          },
        });

      localStorage.setItem('token', response.data.token);
      naviagte('/')
      console.log("login success",response)
        alert("login successfull ")
      } catch (error) {
        alert("something went wrong")
        console.log(error)
      }


    };



  const GoogleSigninsuccess = (response)=>{
    
    const res = jwtDecode(response.credential)
    setuser(res)

    console.log(res.name)    
    console.log(res)    

  }

  return (
      
      <Paper elevation={3} style={{ display:"flex" , flexDirection:"column" , width:"50vw" , padding:"20px"}} >

      <TextField style={{ margin:"10px" }} id="outlined-basic" label="Email" variant="outlined" name='Email' value={email} onChange={(e)=>setemail(e.target.value)} error={email && !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)} helperText={email && !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email) ? "Email should be valid" : ""}/>
      <TextField style={{ margin:"10px" }} id="outlined-basic" label="Password" variant="outlined" name='Password' value={password} onChange={(e)=>setpassword(e.target.value)}/>
      <Button style={{ margin:"10px" }} variant="outlined"   onClick={handleSubmit}>Login</Button>
      <GoogleLoginComponent loginSuccess={GoogleSigninsuccess}/>
      <Typography style={{ margin:"10px" }} color="blue" variant='h6' onClick={()=>naviagte('/signup')}>Not a user? signup from here</Typography>
      </Paper>

  );

}

export default Login