import React, { Component } from 'react';
import { Home } from './components/Home';
import { Result } from './components/Result';
import { Route, Link, BrowserRouter as Router, Switch } from 'react-router-dom';
import './App.css'

export default class App extends Component {
  static displayName = App.name;

  constructor(props){
    super(props);
    this.state = {
      token: -1,
      selectedCities: [],
      selectedMethods: [{}],
      amout_city: 0,
      isRandom: false
    };

    this.onValidateForm = this.onValidateForm.bind(this)
  }

  onValidateForm(token, selectedCities, selectedMethods, isRandom, amout_city){
    this.setState({token: token, selectedCities: selectedCities, selectedMethods: selectedMethods, isRandom: isRandom, amout_city: amout_city})
    console.log(this.state)
  }

  render () {
    return (
      <Router>
        <Switch>
          <Route exact path ="/" ><Home onValidateForm = {this.onValidateForm}/></Route>
          <Route path ="/compare"><Result payload = {this.state}/></Route>
        </Switch>
      </Router>
    );
  }
}
