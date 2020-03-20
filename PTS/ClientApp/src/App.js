import React, { Component } from 'react';
import { Home } from './components/Home';
import './App.css'

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Home/>
    );
  }
}
