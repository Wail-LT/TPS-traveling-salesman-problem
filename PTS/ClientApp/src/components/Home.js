import React, { Component } from 'react';
import { Spinner } from 'reactstrap';
import MultiSelect from "../List/MultiSelect";
//import MultiSelect from "@khanacademy/react-multi-select";


import './Home.css';

export class Home extends Component {
  
  static displayName = Home.name;

  constructor(props) {
    super(props);
      this.state = {
          cities: [],
          selected: [],
          forecasts: [],
          loading: true
      };
  }

  componentDidMount() {
    this.populateWeatherData();
  }

    renderForecastsTable() {

        const cities = this.state.cities
        const options = cities.map(city => { return { label: city.split("|")[0] + " (" + city.split("|")[1] + ")", value: city } })
        const selected = this.state.selected
        const mselect = (<MultiSelect
            options={options}
            selected={selected}
            onSelectedChanged={selected => this.setState({ selected: selected })}
        />)

        return (
            <div>
              <MultiSelect/>
            <select multiple>
                {cities.map(city =>
                    <option value={city}>{city.split("|")[0] + " (" + city.split("|")[1] + ")"}</option>
                )}
                </select>
            </div>
      /*<table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Date</th>
            <th>Temp. (C)</th>
            <th>Temp. (F)</th>
            <th>Summary</th>
          </tr>
        </thead>
        <tbody>
          {forecasts.map(forecast =>
            <tr key={forecast.date}>
              <td>{forecast.date}</td>
              <td>{forecast.temperatureC}</td>
              <td>{forecast.temperatureF}</td>
              <td>{forecast.summary}</td>
            </tr>
          )}
        </tbody>
      </table>*/
    );
  }

    render() {
    console.log(this.state.cities);
    let contents = this.state.loading
        ? (<div>
            <Spinner color="primary" />
            </div>)
        : this.renderForecastsTable();

    return (
      <div>
        <h1 id="tabelLabel" >Find your way.</h1>
        <p>Compare different alogrithms to find the shortest route for your travel.</p>
        {contents}
      </div>
    );
  }

  async populateWeatherData() {
    const response = await fetch('setup');
    const data = await response.json();
      console.log(this.state.cities);
      this.setState({ cities: data.cityList.map(city => city.m_Item1 + "|" + city.m_Item2), loading: false });

  }
}
