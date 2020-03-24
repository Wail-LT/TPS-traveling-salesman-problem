import React, { Component } from 'react';
import { Spinner, Row, Col, Button } from 'reactstrap';
import Chart from "../chart/Chart";


import './Result.css';

export class Result extends Component {
  static displayName = Result.name;

  constructor(props) {
    super(props);
    this.state = {
      token: props.payload.token,
      selectedCities: props.payload.selectedCities,
      selectedMethods: props.payload.selectedMethods,
      isRandom: props.payload.isRandom,
      amout_city: props.payload.amout_city,
      data: {bestFitnessPerGen: [[]], bestRoutePerMethod:[[{}]]},
      loading: true
    };
  }

  componentDidMount(){
    if(this.state.token !=-1)
      this.getResult()
  }

  async getResult()
  {
    let endPoint = this.state.isRandom?"api/randomresult":"api/result"

    let json = this.state.isRandom?
    {
      sessionToken: this.state.token,
      amountOfCities: this.state.amout_city,
      selectionMethods: this.state.selectedMethods.map(methode => methode.name),
      mutationFactors: this.state.selectedMethods.map(methode => methode.factor),
      nbGenerations: 100
    }:
    {
      sessionToken: this.state.token,
      cities: this.state.selectedCities,
      selectionMethods: this.state.selectedMethods.map(methode => methode.name),
      mutationFactors: this.state.selectedMethods.map(methode => methode.factor),
      nbGenerations: 100
    }
    
    const response = await fetch(endPoint, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(json),
      })
      var rps = await response.json()
      
      var data = {bestFitnessPerGen: [], bestRoutePerMethod: undefined}
      rps.bestFitnessPerGen.forEach(element=>{
        data.bestFitnessPerGen.push(element.map((fitness, gen) => {return {fitness: Math.round((fitness + Number.EPSILON) / 10 ) / 100 , gen: gen}}))
      })
      data.bestRoutePerMethod = rps.bestRoutePerMethod
      
      console.log(data)
      this.setState({data: data, loading: false})
  }

  render() {
    console.log(this.state.data.bestRoutePerMethod)
    let loadingSpinner = (
      <div>
        <Spinner color="primary" />
      </div>);
    
    let body = (
      <div>fini</div>
    )
    let renderLineChart = (<Row className="row-center">
      {
        this.state.data.bestRoutePerMethod.map((bestRoute, index)=>
          (
            <Col key={index} xs={(index%2) && { size: 5 } ||{ size: 5, offset: 1 }}>
              <Chart method={this.state.selectedMethods[index]} data={this.state.data.bestFitnessPerGen[index]} bestRoute={bestRoute}/>
            </Col> 
          )
        )
      }
    </Row>

    );
    let contents = this.state.loading ? loadingSpinner : renderLineChart;
    return (
      <div className="home">
        <h1 id="tabelLabel" >Find your way.</h1>
        <p>Compare different alogrithms to find the shortest route for your travel.</p>
        {contents}
      </div>  
    );
  }
}
