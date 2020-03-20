import React, { Component } from 'react';
import { Spinner, Row, Col, Button } from 'reactstrap';
import MultiSelect from "../List/MultiSelect";
import SelectMethode from "../Methode/SelectMethode";


import './Home.css';

export class Home extends Component {

  static displayName = Home.name;

  constructor(props) {
    super(props);
    this.state = {
      selectedCities: [],
      selectedMethodes: [{}],
      value: 0.3,
      loading: true
    };
    this.token = -1;
    this.methodes = [];
    this.unselectCity = this.unselectCity.bind(this);
    this.addSelectedMethode = this.addSelectedMethode.bind(this);
    this.updateSelectedMethode = this.updateSelectedMethode.bind(this);
    this.sendForm = this.sendForm.bind(this);
  }

  componentDidMount() {
    this.setup();
  }

  unselectCity(ville) {
    this.setState({ selectedCities: this.state.selectedCities.filter(v => ville.name + "." + ville.zip != v.name + "." + v.zip) })
  }

  addSelectedMethode() {
    this.state.selectedMethodes.push({ name: this.methodes[0].name, factor: this.methodes[0].factor });
    this.setState({ selectedMethodes: this.state.selectedMethodes });
  }

  updateSelectedMethode(methode, index) {
    if (methode !== undefined)
      this.state.selectedMethodes[index] = methode;
    else
      this.state.selectedMethodes.splice(index, 1);
    this.setState({ selectedMethodes: this.state.selectedMethodes });
  }

  render() {
    let loadingSpinner = (
      <div>
        <Spinner color="primary" />
      </div>);

    let body = (
      <div id="content">
        <Row>
          <Col xs={{ size: 10, offset: 1 }}>
            <div className="label">Cities to visite :</div>
            <MultiSelect onDelete={this.unselectCity} selectedCities={this.state.selectedCities} />
          </Col>
        </Row>

        <Row>
          <Col xs={{ size: 10, offset: 1 }}>
            <div className="label">Selection Methodes</div>
          </Col>
        </Row>
        {this.state.selectedMethodes.map((methode, index) => (
          <Row className="selectMethode"  key={index}>
            <Col xs={{ size: 10, offset: 1 }}>
              <SelectMethode className="selectMethode" selectedMethode={methode} methodes={this.methodes} onUpdate={(m) => this.updateSelectedMethode(m, index)} />
            </Col>
            <Col xs="1">
              {index != 0 && (<span className="deleteIcon unselectable"><img src="plus.svg" onClick={() => this.updateSelectedMethode(undefined, this.index)} /></span>)}
            </Col>
          </Row>))}

        <div className="addIcon unselectable"><img src="plus.svg" onClick={this.addSelectedMethode} /></div>
        <Row className="validateForm">
          <Col xs={{ size: 10, offset: 1 }}>
            <Button color="primary" onClick={this.sendForm}>Compare</Button>
          </Col>
        </Row>
      </div>
    );
    let contents = this.state.loading ? loadingSpinner : body;

    return (
      <div className="home">
        <h1 id="tabelLabel" >Find your way.</h1>
        <p>Compare different alogrithms to find the shortest route for your travel.</p>
        {contents}
      </div>
    );

  }

  sendForm(){
    let json = {
      token: this.token,
      cities: this.state.selectedCities,
      methodes: this.state.selectedMethodes
    }
    console.log(json);
  }

  async setup() {
    const response = await fetch('api/setup');
    const data = await response.json();
    this.token = data.token;
    this.methodes = data.selectionMethodes.map(methode => { return { name: methode.m_Item1, factor: methode.m_Item2 } })
    this.setState({ selectedMethodes: [{ name: this.methodes[0].name, factor: this.methodes[0].factor }], loading: false });
  }
}
