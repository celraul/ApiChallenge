import http from 'k6/http';
import { check, sleep } from 'k6';

export let options = {
    stages: [
      { duration: '1m', target: 20 }, // ramp up to x users
      { duration: '1m', target: 20 }, // stay at x users
      { duration: '1m', target: 0 },  // ramp down
    ],
    // vus: 10,         // Number of virtual users
    // duration: '60s', // Duration of the test
};

let endpoint = 'https://localhost:7226/api'
let version = 'v1';
let boardID = '682c7b083e47fe4a8b9d7550';

export default function () {
    let res = http.get(`${endpoint}/${version}/Board/${boardID}/finalState`);

    check(res, {
        'status is 200': (r) => r.status === 200,
        'body is not empty': (r) => r.body.length > 0,
    });

    //sleep(1); 
}