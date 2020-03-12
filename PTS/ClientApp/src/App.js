import React, { Component } from 'react';
import { Route, Router } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { FetchData } from './components/FetchData';
import { Counter } from './components/Counter';
import { Pts } from './components/Pts';

import './custom.css'

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Home/>
    );
  }
}
