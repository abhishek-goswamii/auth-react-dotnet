import React from 'react'
import { GoogleOAuthProvider, GoogleLogin } from '@react-oauth/google';

const GoogleLoginComponent = ({loginSuccess}) => {
  return (
    <>
    
    <GoogleOAuthProvider clientId="649290839564-lkbpqvo5h6c4cqn8c2kp3jhri8cnvuq8.apps.googleusercontent.com">
        <GoogleLogin

          onSuccess={credentialResponse => {
            loginSuccess(credentialResponse)
          }}
          onError={() => {
            alert("something went wrong")
            console.log('GoogleLoginComponent Failed');
          }}
        
        />

      </GoogleOAuthProvider>
    </>
  )
}

export default GoogleLoginComponent









