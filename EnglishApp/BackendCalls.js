const backend = () => {
	fetch('http://jsonplaceholder.typicode.com/users', {
		method: 'GET',
		headers: {
			'Accept': 'application/json',
			'Content-Type': 'application/json',
		},
	})
	.then((response) => response.json())
	.then((res) => {
		let i;
		for(i = 0; i < res.length; i++)
			alert(res[i].name);  
	})
}

export default backend;