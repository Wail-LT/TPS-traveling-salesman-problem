import React, { Component } from 'react';
import { Spinner, Row, Col, Button } from 'reactstrap';
import MultiSelect from "../List/MultiSelect";
import { FormGroup, InputGroup, Label, Input } from 'reactstrap';
import SelectMethode from "../Methode/SelectMethode";
import { Link, useHistory } from 'react-router-dom';


import './Home.css';

export class Home extends Component {

  static displayName = Home.name;

  constructor(props) {
    super(props);
    this.state = {
      selectedCities: [],
      selectedMethods: [{}],
      enough_city: false,
      isRandom: false,
      amout_city: 0,
      loading: true
    };
    this.token = -1;
    this.methodes = [];
    this.onValidateForm = props.onValidateForm;
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
    this.state.selectedMethods.push({ name: this.methodes[0].name, factor: this.methodes[0].factor });
    this.setState({ selectedMethods: this.state.selectedMethods });
  }

  updateSelectedMethode(methode, index) {
    if (methode !== undefined)
      this.state.selectedMethods[index] = methode;
    else
      this.state.selectedMethods.splice(index, 1);
    this.setState({ selectedMethods: this.state.selectedMethods });
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
            <MultiSelect onDelete={this.unselectCity} selectedCities={this.state.selectedCities} onValidate={() => this.setState({ enough_city: this.state.selectedCities.length > 4 })} />
          </Col>
        </Row>
        <Row>
          <Col xs={{ size: 3, offset: 2 }}>
            <FormGroup>
              <InputGroup>
                <Input type="checkbox" id="checkbox" onChange={()=>
                  this.setState({isRandom: !this.state.isRandom})} />
                <Label for="checkbox" check>Select random cities</Label>
                <Input placeholder="Amount" id="cities_number" value={this.state.amout_city} onChange={(e) => this.setState({amout_city: e.target.value})} />
              </InputGroup>
            </FormGroup>
          </Col>
        </Row>
        <Row>
          <Col xs={{ size: 10, offset: 1 }}>
            <div className="label">Selection Methodes</div>
          </Col>
        </Row>
        {this.state.selectedMethods.map((methode, index) => (
          <Row className="selectMethode" key={index}>
            <Col xs={{ size: 10, offset: 1 }}>
              <SelectMethode className="selectMethode" selectedMethode={methode} methodes={this.methodes} onUpdate={(m) => this.updateSelectedMethode(m, index)} />
            </Col>
            <Col xs="1">
              {index != 0 && (<span className="deleteIcon unselectable"><img src="plus.svg" onClick={() => this.updateSelectedMethode(undefined, this.index)} /></span>)}
            </Col>
          </Row>))}

        <div className="addIcon unselectable"><img src="plus.svg" onClick={this.addSelectedMethode} /></div>
        {(this.state.enough_city || this.state.isRandom && this.state.amout_city > 4 ) &&
          (<Row className="validateForm">
            <Col xs={{ size: 10, offset: 1 }}>
              <Link to="/compare"><Button color="primary" onClick={this.sendForm}>Compare</Button></Link>
            </Col>
          </Row>)}

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

  sendForm() {
    this.onValidateForm(this.token, this.state.selectedCities, this.state.selectedMethods, this.state.isRandom, this.state.amout_city)
  }

  async setup() {
    const response = await fetch('api/setup');
    const data = await response.json();
    this.token = data.token;
    this.methodes = data.selectionMethodes.map(methode => { return { name: methode.m_Item1, factor: methode.m_Item2 } })
    this.setState({ selectedMethods: [{ name: this.methodes[0].name, factor: this.methodes[0].factor }], loading: false });
  }
}
