import React, { useEffect } from 'react'
import { baseurl } from '../baseurl';


const AccounVerificationPage = () => {
  
    useEffect(() => {
        // Function to extract token from URL
        const getTokenFromURL = () => {
          const params = new URLSearchParams(window.location.search);
          return params.get('token');
        }
    
        const token = getTokenFromURL();
        fetch(`${baseurl}/api/verify`, {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${token}`
            }
        }).then(response => {
            if (response.status === 200) {
                alert("user registered and verified successfully")
            } else if (response.status === 400) {
                alert("user already exist with this mail")
            }
        }).catch(error => {
            alert(error)
            console.error(error);
        });


    })

    return (
    <div>AccounVerificationPage</div>
  )
}

export default AccounVerificationPage